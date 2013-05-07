using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
	public new string name;

	public event EventHandler TurnOver;

	public NetworkViewID viewID;
	public NetworkPlayer owner;
	public string network_id;
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
	
	// THIS MUST BE SET
	public int team = 0;
	private
	GameObject indicator;
	Unit target_unit;
	List<Unit> target_units = new List<Unit> ();
	BattleManager bm;
    #endregion
	
    #region Methods
	protected virtual void OnTurnOver (EventArgs e)
	{
		if (TurnOver != null)
			TurnOver (this, e);
	}
	
	public void Awake ()
	{
	}
	
	// Use this for initialization
	public virtual void Start ()
	{
		_class = gameObject.GetComponent<UnitClass> ();
		current_ap = 0;
		network_id = Network.player.guid;
		bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
	}
	
	public void OnMouseDown ()
	{
	}
	
	// Update is called once per frame
	public virtual void FixedUpdate ()
	{		
		
	}
	
	
	// used mostly for start of turn effects
	[RPC]
	public void start_turn (string nvid)
	{
		if (is_bleeding && bleed_timer > 0) {
			take_damage (bleed_dmg);
			bleed_timer--;
			if (bleed_timer == 0) {
				is_bleeding = false;	
				Destroy (transform.FindChild ("poison_effect(Clone)").gameObject);
			}
		}
	}
	
	[RPC]
	public void set_material(int mat) {
		if (mat == 0)
			renderer.material = Resources.Load("Materials/selected_unit_material") as Material;
		else if (mat == 1)
			renderer.material = Resources.Load("Materials/player_one_material") as Material;
		else if (mat == 2)
			renderer.material = Resources.Load("Materials/player_two_material") as Material;
	}
	
	[RPC]
	public void set_current_ap(int _cur_ap) {
		current_ap = _cur_ap;	
	}
	
	[RPC]
	public void show_menu_rpc(string nvid){
		if (nvid == network_id)
			show_menu();
	}
	
	[RPC]
	public void set_team(int _team) {
		team = _team;
	}
	
	[RPC]
	public void set_owner(NetworkPlayer player) {
		owner = player;
		network_id = player.guid;
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
	
	public IEnumerator do_action (string _ind, ACTIONS action)
	{
		indicator = GameObject.Instantiate (Resources.Load ("Prefabs/" + _ind) as GameObject, transform.position, transform.rotation) as GameObject;
		Vector3 mouse_input = new Vector3 (0, 0, 0);
		Vector3 target_destination = new Vector3 (0, 0, 0);
		bool performed_action = false;
		bool action_success = true;
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
			indicator.transform.localScale = new Vector3 (action_range, action_range, action_range);
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
			Ray ray = Camera.allCameras [0].ScreenPointToRay (mouse_input);
			float hitdist = 0.0f;
			
			if (playerPlane.Raycast (ray, out hitdist)) {
				target_destination = ray.GetPoint (hitdist);
				// the + .3 is to account for some isometric confusion.
				if (Vector3.Distance (transform.position, target_destination) <= action_range + .3f) {
					Quaternion targetRotation = Quaternion.LookRotation (target_destination - transform.position);
					transform.rotation = targetRotation;
					if (action == ACTIONS.ATTACK) {
						Collider[] hitColliders = Physics.OverlapSphere (target_destination, 1);
						print (hitColliders.Length);
						if (hitColliders.Length > 0) {
							foreach (Collider q in hitColliders) {
								// grabs first unit
								if (q.GetComponent<Unit> () != null) {
									target_unit = q.GetComponent<Unit> ();	
								}
							}
							if (target_unit != null) {
								if (target_unit.gameObject != gameObject) {
									performed_action = true;
									action_success = true;
								} else {
									print ("failed can't attack self");
									print (target_unit);
									performed_action = true;
									action_success = false;
								}
							} else {
								print ("failed no units");
								performed_action = true;
								action_success = false;
							}
						}
					}
					
					if (action == ACTIONS.SPELL) {
						Collider[] hitColliders = Physics.OverlapSphere (target_destination, _class.spell.spell_area);
						print (hitColliders.Length);
						
						// TODO: CHECK FOR SPELL TYPE AND DO DIFFERENT THINGS ACCORDINGLY
						// SELF: DON'T SHOW INDICATOR, JUST DO SPELL
						// ENEMY: CHECK IF COLLIDER Q IS ENEMY OR ALLY
						// ALLY: SAME
						
						switch (_class.spell.type) {
						case Spell.TYPE.ALLY:
							if (hitColliders.Length > 0) {
								target_units.Clear ();
								foreach (Collider q in hitColliders) {
										
									if (q.GetComponent<Unit> () != null && q.tag == tag) {
										target_units.Add (q.GetComponent<Unit> ());
										action_success = true || action_success;
									} else {
										print ("failed, not ally " + q);
										action_success = false || action_success;
									}
								}	
								performed_action = true;
							}
							break;
						case Spell.TYPE.ENEMY:
							print (hitColliders.Length);
							if (hitColliders.Length > 0) {
								target_units.Clear ();
								foreach (Collider q in hitColliders) {
									print (q);	
									if (q.GetComponent<Unit> () != null) {
										if (q.tag != tag) {
											target_units.Add (q.GetComponent<Unit> ());
											action_success = true || action_success;
										} else {
											print ("failed, not	enemy " + q);
											action_success = false || action_success;
										}
									} else {
										print ("failed, not unit " + q);
										action_success = false || action_success;
									}
								}
								performed_action = true;
							}
							break;
						case Spell.TYPE.SELF:
							if (hitColliders.Length > 0) {
								target_units.Clear ();
								foreach (Collider q in hitColliders) {
										
									if (q.GetComponent<Unit> () != null) {
										if (q.gameObject == gameObject) {
											target_units.Add (q.GetComponent<Unit> ());
											action_success = true || action_success;
										} else {
											print ("failed, not self");
											print (target_unit);
											action_success = false || action_success;
										}
									} else {
										print ("failed, not a unit " + q);
										action_success = false || action_success;
									}
								}
								performed_action = true;
							}
							break;
						}
					}
					
					performed_action = true;
				}
			}
		} while (!performed_action);
		
		Destroy (indicator);
		
		if (action_success)
			current_ap -= 1;
		
		menu_showing = false;
		
		if (action_success) {
			switch (action) {
			case ACTIONS.MOVE:
				move_to (target_destination, 1);
				break;
			case ACTIONS.WAIT:
				OnTurnOver (EventArgs.Empty);
				current_ap = 0;
				break;
			case ACTIONS.ATTACK:
				attack_target (target_unit);
				break;
			case ACTIONS.SPELL:
				cast_spell_on_target (ref target_units);
				break;
			default:
				break;
			}
		}
		
		if (current_ap > 0) {
			show_menu();
			yield return null;
		} else {
			OnTurnOver (EventArgs.Empty);
			yield return null;
		}
	}
	
	public IEnumerator get_input ()
	{
		bool input_recieved = false;
		
		while (!input_recieved) {
			Plane playerPlane = new Plane (Vector3.up, transform.position);
			Ray ray = Camera.allCameras [0].ScreenPointToRay (Input.mousePosition);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast (ray, out hitdist)) {
				transform.LookAt (ray.GetPoint (hitdist), Vector3.up);
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
	
	public virtual void move_to (Vector3 pos, float t)
	{
		Vector3 start = transform.position;
		Vector3 end = pos;
		
		iTween.MoveTo (gameObject, iTween.Hash ("x", pos.x, "y", pos.y, "z", pos.z, "easeType", "easeInOutQuad"));
		
		if (current_ap > 0) {
			show_menu();
		}
	}
	
	public virtual void attack_target (Unit u)
	{
		print ("attacking targetting: " + u);
		if (boost_dmg) {
			print ("damage boosted");
			u.take_damage (_class.damage * (boost_percent / 100));
			GameObject g = GameObject.Instantiate (Resources.Load ("Prefabs/Power Burst"), u.transform.position, Quaternion.identity) as GameObject;
			Destroy (transform.FindChild ("theif_charged(Clone)").gameObject);
		} else
			u.take_damage (_class.damage);
	}
	
	public virtual void cast_spell_on_target (ref List<Unit> u)
	{
		
		foreach (Unit _u in u) {
			print ("casting spell on target: " + _u);
			_u.receive_spell (_class.spell, this);
		}
		
		u.Clear ();
	}
	
	public virtual void take_damage (int dmg)
	{
		if (sharing_dmg) {
			print ("sharing damage");
			sharing_dmg_with.take_damage (dmg * (sharing_percent / 100));
			_class.hp -= (int)(dmg * (1 - (sharing_percent / 100)));
			sharing_dmg = false;
			sharing_dmg_with = null;
			sharing_percent = 0;
			Destroy (transform.FindChild ("Ground Vortex(Clone)").gameObject);
		} else {
			_class.hp -= (int)(dmg - .1f * _class.physical_defence);
		}
	}
	
	public virtual void receive_spell (Spell s, Unit caster)
	{
		print ("Receiving spell " + s + " from: " + caster);
		s.perform_spell (caster, this);	
	}
    #endregion
}