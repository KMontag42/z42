using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Unit : MonoBehaviour
{
    #region Fields
	public enum ACTIONS
	{
		MOVE,
		ATTACK,
		DEFEND,
		SPELL,
		WAIT
	};
	public int current_ap;
	public int speed;
	public UnitClass _class;
	
	public Path path;
	public Seeker seeker;
	public int currentWaypoint;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
	public CharacterController controller;

	public event EventHandler TurnOver;

	public NetworkViewID viewID;
	public NetworkPlayer owner;
	public int special_id;
	public bool menu_showing = false;
	public bool defending = false;
	public LayerMask grab_layer;
	
	// for paladin spell
	public bool sharing_dmg;
	public Unit sharing_dmg_with;
	public int sharing_percent;
	
	// for theif spell
	public bool boost_dmg;
	public int boost_percent;
	
	// for gunner spell
	public bool is_bleeding;
	public int bleed_dmg;
	public int bleed_timer;
	public List<Effect> effects = new List<Effect> ();
	
	// THIS MUST BE SET
	public int team = 0;
	GameObject indicator;
	public Unit target_unit;
	public List<Unit> target_units = new List<Unit> ();
	BattleManager bm;
    #endregion
	
    #region Methods
	#region RPCS
	[RPC]
	protected virtual void OnTurnOver ()
	{
		if (TurnOver != null)
			TurnOver (this, EventArgs.Empty);
	}
	
	[RPC]
	public void start_turn ()
	{
//		foreach (Effect e in effects) {
//			if (e.type == Effect.TYPE.DPT)
//				e.apply_effect(this);		
//		}
		if (is_bleeding && bleed_timer > 0) {
			networkView.RPC("take_damage", RPCMode.All, bleed_dmg);
			bleed_timer--;
			if (bleed_timer == 0) {
				is_bleeding = false;	
				Destroy (transform.FindChild ("poison_effect(Clone)").gameObject);
			}
		}
		(GameObject.Find("3D Camera") as GameObject).GetComponent<iTweenFollow>().target = transform;
		show_menu();
	}
	
	[RPC]
	public void set_material (int mat)
	{
		if (mat == 0)
			renderer.material = Resources.Load ("Materials/selected_unit_material") as Material;
		else if (mat == 1)
			renderer.material = Resources.Load ("Materials/player_one_material") as Material;
		else if (mat == 2)
			renderer.material = Resources.Load ("Materials/player_two_material") as Material;
	}
	
	[RPC]
	public void request_set_current_ap(int _cur_ap) {
		networkView.RPC("set_current_ap", RPCMode.All, _cur_ap);	
	}
	
	[RPC]
	public void set_current_ap (int _cur_ap)
	{
		current_ap = _cur_ap;	
	}
	
	[RPC]
	public void set_team (int _team)
	{
		team = _team;
		if (team == 1)
			tag = "team_1";
		else if (team == 2)
			tag = "team_2";
		else
			print ("error in set_team from: " + Network.player);
	}
	
	[RPC]
	public void set_owner (NetworkPlayer player)
	{
		System.Random r = new System.Random();
		owner = player;
		special_id = r.Next();
	}
	
	[RPC]
	public virtual void request_theif_spell_rpc(int boost_bool, int boost_per, string effect) {
		// validation here
		networkView.RPC("theif_spell_rpc", RPCMode.All, boost_bool, boost_per, effect);
	}
	
	[RPC]
	public void theif_spell_rpc (int boost_bool, int boost_per, string effect)
	{
		if (boost_bool == 1) {
			boost_dmg = true;
			boost_percent = boost_per;
			
			GameObject g = GameObject.Instantiate (Resources.Load ("Prefabs/" + effect), transform.position, Quaternion.identity) as GameObject;
			g.transform.parent = transform;
		} else if (boost_bool == 2) {
			boost_dmg = false;
			boost_percent = 0;
			try {
				Destroy(GameObject.Find("theif_charged(Clone)"));	
			} catch (NullReferenceException e) {
				print (e + " from: " + Network.player);
			}
		} else {
			print ("fail set_boost_dmg");
		}		
	}
	
	[RPC]
	public void show_team_frame_rpc ()
	{
		Camera.main.GetComponent<GUIOverlay>().showTeamFrame();
	}
	
	[RPC]
	public virtual void take_damage (int dmg)
	{
		if (sharing_dmg) {
			sharing_dmg_with.networkView.RPC ("request_to_damage", RPCMode.Server, (dmg * (sharing_percent / 100)));
			_class.hp -= (int)(dmg * (1 - (sharing_percent / 100)));
			sharing_dmg = false;
			sharing_dmg_with = null;
			sharing_percent = 0;
			Destroy (transform.FindChild ("protector_effect(Clone)").gameObject);
		} else {
			_class.hp -= (int)(dmg - .1f * _class.physical_defence);
		}
	}
	
	[RPC]
	public virtual void move_to (Vector3 pos, float t)
	{		
//		NavMeshPath path = new NavMeshPath();
//		if (seeker.CalculatePath(pos, path)) {
//			
//		}
//		seeker.SetDestination(pos);
//		seeker.Stop();
		seeker.StartPath(transform.position, pos, OnPathComplete);
		
		networkView.RPC ("request_set_current_ap", RPCMode.Server, current_ap - 1);
		
		if (current_ap > 0) {
			if (Network.player == owner)
				show_menu();
		} else {
			if (Network.player == owner)
				networkView.RPC ("OnTurnOver", RPCMode.Server);
		}
	}
	
	[RPC]
	public virtual void request_protector_spell_rpc(int percent, string effect, int protect_bool, int target_id) {
		//validate here
		networkView.RPC("protector_spell_rpc", RPCMode.All, percent, effect, protect_bool, target_id);
	}
	
	[RPC]
	public virtual void protector_spell_rpc(int percent, string effect, int protect_bool, int target_id) {
		if (protect_bool == 1){
			sharing_dmg = true;
			sharing_percent = percent;
			// this works because this is only called from the target of the active player
			// le sigh...
			sharing_dmg_with = bm.gm.players[target_id];
			GameObject g = GameObject.Instantiate(Resources.Load("Prefabs/"+effect), transform.position, Quaternion.identity) as GameObject;
			g.transform.parent = transform;	
		} else if (protect_bool == 2) {
			sharing_dmg = false;
			sharing_percent = 0;
			try {
				Destroy(GameObject.Find("protector_effect(Clone)"));	
			} catch (NullReferenceException e) {
				print (e + " from: " + Network.player);
			}
		} else {
			print ("failed protector_spell_rpc from: "+Network.player);	
		}
	}
	
	[RPC]
	public virtual void request_healer_spell_rpc(string effect) {
		//validate here
		networkView.RPC("healer_spell_rpc", RPCMode.All, effect);
	}
	
	[RPC]
	public virtual void healer_spell_rpc(string effect) {
		is_bleeding = false;
		bleed_dmg = 0;
		bleed_timer = 0;
		GameObject g = GameObject.Instantiate(Resources.Load("Prefabs/"+effect), transform.position, Quaternion.identity) as GameObject;
		try {
			GameObject.Destroy(transform.FindChild("poison_effect(Clone)").gameObject);
		} catch (NullReferenceException e) {
			print (e + " from: " + Network.player);	
		}
	}
	
	[RPC]
	public virtual void request_gunner_spell_rpc(int damage, string effect, string target_effect, Vector3 caster_pos, int bleed_bool) {
		//validate here
		networkView.RPC("gunner_spell_rpc", RPCMode.All, damage, effect, target_effect, caster_pos, bleed_bool);
	}
	
	[RPC]
	public virtual void gunner_spell_rpc(int damage, string effect, string target_effect, Vector3 caster_pos, int bleed_bool) {
		if (bleed_bool == 1) {
			is_bleeding = true;
			bleed_dmg = damage;
			bleed_timer = 3;
			GameObject g = GameObject.Instantiate (Resources.Load ("Prefabs/" + effect), caster_pos, Quaternion.identity) as GameObject;
			iTween.MoveTo (g, transform.position, .2f);
			GameObject p = GameObject.Instantiate(Resources.Load("Prefabs/"+target_effect), transform.position, Quaternion.identity) as GameObject;
			p.transform.parent = transform;	
		} else if (bleed_bool == 2) {
			is_bleeding = false;
			bleed_dmg = 0;
			bleed_timer = 0;
			try {
				Destroy(GameObject.Find("poison_effect(Clone)"));	
			} catch (NullReferenceException e) {
				print (e + " from: " + Network.player);
			}
		} else {
			print ("failed protector_spell_rpc from: "+Network.player);	
		}
	}
	
	[RPC]
	public virtual void request_to_spawn_prefab_on(string prefab_name) {
		//validation here
		networkView.RPC("spawn_prefab_on", RPCMode.All, prefab_name);
	}
	
	[RPC]
	public virtual void spawn_prefab_on(string prefab_name) {
		if (Network.isServer) {
			GameObject g = Network.Instantiate (Resources.Load (prefab_name), transform.position, Quaternion.identity, team) as GameObject;
		}
	}
	
	[RPC]
	public virtual void submit_move(Vector3 pos, float t) {
		// check for validation here
		networkView.RPC("move_to", RPCMode.All, pos, t);
	}
	
	[RPC]
	public virtual void request_to_damage(int dmg) {
		// validation here
		networkView.RPC("take_damage", RPCMode.All, dmg);
	}
	
	[RPC]
	public virtual void set_view_id(NetworkViewID viewID) {
		networkView.viewID = viewID;	
	}
	#endregion
	
	void Awake ()
	{
	}
	
	// Use this for initialization
	public virtual void Start ()
	{
		_class = gameObject.GetComponent<UnitClass> ();
		current_ap = 0;
		bm = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
		seeker = GetComponent<Seeker>();
		name = name + " team: " + team;
		
		controller = GetComponent<CharacterController>();
	}
	
	public void OnMouseDown ()
	{
	}
	
	// Update is called once per frame
	public virtual void FixedUpdate ()
	{		
		if (menu_showing) {
			Camera camera_3d = Camera.allCameras [0];
			Vector3 pos = camera_3d.WorldToScreenPoint (transform.position);
			GUIOverlay.showTooltip (this, pos.x, Screen.height - pos.y);	
		}
	}
	
	public IEnumerator do_action (string _ind, ACTIONS action)
	{
		indicator = GameObject.Instantiate (Resources.Load ("Prefabs/" + _ind) as GameObject, transform.position, transform.rotation) as GameObject;
		Vector3 mouse_input = new Vector3 (0, 0, 0);
		Vector3 target_destination = new Vector3 (0, 0, 0);
		bool performed_action = false;
		bool action_success = false;
		int action_range;
		
		switch (action) {
		case ACTIONS.MOVE:
			action_range = _class.move_range;
			indicator.transform.localScale = new Vector3 (action_range / 2, action_range / 2, action_range / 2);
			break;
		case ACTIONS.ATTACK:
			action_range = _class.attack_range;
			indicator.transform.localScale = new Vector3 (action_range / 3, action_range / 3, action_range / 3);
			break;
		case ACTIONS.SPELL:
			action_range = _class.spell_range;
			indicator.transform.localScale = new Vector3 (action_range / 3, action_range / 3, action_range / 3);
			break;
		default:
			action_range = 5;
			indicator.transform.localScale = new Vector3 (action_range / 3, action_range / 3, action_range / 3);
			break;
		}
		
		do {
			yield return StartCoroutine(get_input());
			mouse_input = Input.mousePosition;
			
			Plane playerPlane = new Plane (Vector3.up, transform.position);
			//Ray ray = (GameObject.Find("3D Camera") as Camera).ScreenPointToRay(mouse_input);
			Ray ray = Camera.allCameras [0].ScreenPointToRay (mouse_input);
			RaycastHit hitinfo;
			float hitdist = 0.0f;
			
			if (Physics.Raycast (ray, out hitinfo, 1 << 8)) {
				target_destination = hitinfo.point;
				// the + .3 is to account for some isometric confusion.
				if (Vector3.Distance (transform.position, target_destination) <= action_range + .3f) {
					Quaternion targetRotation = Quaternion.LookRotation (target_destination - transform.position);
					transform.rotation = targetRotation;
					Collider[] hitColliders;
					switch (action) {
					case ACTIONS.ATTACK:
						hitColliders = Physics.OverlapSphere (target_destination, 1, 1 << 8);
						if (hitColliders.Length > 0) {
							foreach (Collider q in hitColliders) {
								// grabs first unit
								if (q.GetComponent<Unit> () != null) {
									target_unit = q.GetComponent<Unit> ();
									break;
								}
							}
							if (target_unit != null) {
								if (target_unit.gameObject != gameObject) {
									performed_action = true;
									action_success = true;
								} else {
									print ("failed can't attack self");
									performed_action = true;
									action_success = false;
								}
							} else {
								print ("failed no units");
								performed_action = true;
								action_success = false;
							}
						}
						break;
					case ACTIONS.SPELL:
						hitColliders = Physics.OverlapSphere (target_destination, _class.spell.spell_area, 1 << 8);
						
						switch (_class.spell.type) {
						case Spell.TYPE.ALLY:
							if (hitColliders.Length > 0) {
								target_units.Clear ();
								foreach (Collider q in hitColliders) {
									Unit q_unit = q.GetComponent<Unit>();
									if (q_unit != null && q_unit.team == team) {
										target_units.Add (q_unit);
										action_success |= true;
									} else {
										print ("failed, not ally " + q_unit);
										action_success |= false;
									}
								}	
								performed_action = true;
							}
							break;
						case Spell.TYPE.ENEMY:
							if (hitColliders.Length > 0) {
								target_units.Clear ();
								foreach (Collider q in hitColliders) {
									Unit q_unit = q.GetComponent<Unit>();
									if (q_unit != null) {
										if (q_unit.team != team) {
											target_units.Add (q_unit);
											action_success |= true;
										} else {
											print ("failed, not	enemy " + q_unit);
											action_success |= false;
										}
									} else {
										print ("failed, not unit " + q_unit);
										action_success |= false;
									}
								}
								performed_action = true;
							}
							break;
						case Spell.TYPE.SELF:
							if (hitColliders.Length > 0) {
								target_units.Clear ();
								target_units.Add (this);
								action_success = true;
								performed_action = true;
							}
							break;
						}
						break;
					case ACTIONS.MOVE:
						action_success = true;
						break;
					case ACTIONS.WAIT:
						action_success = true;
						break;
					}
					
					performed_action = true;
				}
			}
		} while (!performed_action);
		
		Destroy (indicator);
		
		menu_showing = false;
		
		if (action_success) {
			switch (action) {
			case ACTIONS.MOVE:
				networkView.RPC ("submit_move", RPCMode.Server, target_destination, 1.0f);
				break;
			case ACTIONS.WAIT:
				networkView.RPC ("OnTurnOver", RPCMode.Server);
				current_ap = 0;
				break;
			case ACTIONS.ATTACK:
				attack_target (target_unit);
		
				if (current_ap > 0) {
					if (Network.player == owner) {
						networkView.RPC ("request_set_current_ap", RPCMode.Server, current_ap - 1);
						show_menu ();
					}
					yield return null;
				} else {
					if (Network.player == owner)
						networkView.RPC ("OnTurnOver", RPCMode.Server);
					yield return null;
				}
				break;
			case ACTIONS.SPELL:
				cast_spell_on_target (ref target_units);
		
				if (current_ap > 0) {
					if (Network.player == owner) {
						networkView.RPC ("request_set_current_ap", RPCMode.Server, current_ap - 1);
						show_menu ();
					}
					yield return null;
				} else {
					if (Network.player == owner)
						networkView.RPC ("OnTurnOver", RPCMode.Server);
					yield return null;
				}
				break;
			default:
				break;
			}
		} else {
			if (Network.player == owner)
				show_menu ();
		}
	}
	
	public IEnumerator get_input ()
	{
		bool input_recieved = false;
		menu_showing = false;
		
		while (!input_recieved) {
			Plane playerPlane = new Plane (Vector3.up, transform.position);
			Ray ray = Camera.allCameras [0].ScreenPointToRay (Input.mousePosition);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast (ray, out hitdist)) {
				//transform.LookAt (ray.GetPoint (hitdist), Vector3.up);
				indicator.transform.LookAt (ray.GetPoint (hitdist), Vector3.up);
				indicator.transform.Rotate (90, 0, 0);
				if (Input.GetMouseButton (0)) {
					input_recieved = true;
					yield return null;	
				}
				yield return null;
			}
			yield return null;
		}
	}
	
	public virtual void attack_target (Unit target)
	{
		if (boost_dmg) {
			target.networkView.RPC ("request_to_damage", RPCMode.Server, _class.damage * (boost_percent / 100));
			target.networkView.RPC("request_to_spawn_prefab_on", RPCMode.Server, "Prefabs/Power Burst");
			networkView.RPC("request_theif_spell_rpc", RPCMode.Server, 2, 0, "");
		} else
			target.networkView.RPC ("request_to_damage", RPCMode.Server, _class.damage);
	}
	
	public virtual void cast_spell_on_target (ref List<Unit> u)
	{
		foreach (Unit _u in u) {
			_u.receive_spell (_class.spell, this);
		}
		
		u.Clear ();
	}
	
	public virtual void receive_spell (Spell s, Unit caster)
	{
		s.perform_spell (caster, this);	
	}
	
	public void show_menu ()
	{
		if (!menu_showing) {
			Camera camera_3d = Camera.allCameras [0];
			Vector3 pos = camera_3d.WorldToScreenPoint (transform.position);
			GUIOverlay.showTooltip (this, pos.x, Screen.height - pos.y);
			menu_showing = true;
		}
	}
	
	public void OnPathComplete (Path p) {
        Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
			currentWaypoint = 0;
        }
    }
    #endregion
}