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

	public event EventHandler TurnOver;
	public NetworkViewID viewID;
	public bool menu_showing = false;
	
	private
	int move_range;
	int attack_range;
	GameObject indicator;
	int action_points;
	Vector3 last_pos;
	Unit target_unit;
    #endregion
	
    #region Methods
	protected virtual void OnTurnOver (EventArgs e)
	{
		if (TurnOver != null)
			TurnOver (this, e);
	}
	
	// Use this for initialization
	public virtual void Start ()
	{
		_class = gameObject.GetComponent<UnitClass> ();
		move_range = _class.move_range;
		attack_range = _class.attack_range;
		action_points = _class.action_points;
		current_ap = 0;
		last_pos = transform.position;
	}
	
	public void OnMouseDown ()
	{
		//if (networkView.isMine)
		//	show_menu ();
	}
	
	// Update is called once per frame
	public virtual void FixedUpdate ()
	{		
		if (_class.hp <= 0)
			GameObject.Destroy(gameObject);
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
		indicator.transform.localScale = new Vector3 (2, 2, 2);
		Vector3 mouse_input = new Vector3 (0, 0, 0);
		Vector3 target_destination = new Vector3 (0, 0, 0);
		bool performed_action = false;
		int action_range;
		print ("start do_action AP: " + current_ap);
		switch (action) {
		case ACTIONS.MOVE:
			action_range = move_range;
			break;
		case ACTIONS.ATTACK:
			action_range = attack_range;
			break;
		default:
			action_range = 100;
			break;
		}
				
		do {
			yield return StartCoroutine(get_input());
			mouse_input = Input.mousePosition;
			
			Plane playerPlane = new Plane (Vector3.up, transform.position);
			Ray ray = Camera.allCameras [0].ScreenPointToRay (mouse_input);
			float hitdist = 0.0f;
			RaycastHit hit;
			
			if (action == ACTIONS.ATTACK) {
				if (Physics.Raycast(ray, out hit)) {
					print (hit.transform.gameObject);
					target_unit = hit.transform.GetComponent<Unit>();
					if (target_unit.GetType() == typeof(Unit)) {
						performed_action = true;	
					}
				}
			}
 			else {
				if (playerPlane.Raycast (ray, out hitdist)) {
					target_destination = ray.GetPoint (hitdist);
					if (Vector3.Distance (transform.position, target_destination) <= action_range) {
						Quaternion targetRotation = Quaternion.LookRotation (target_destination - transform.position);
						transform.rotation = targetRotation;
						performed_action = true;	
					}
				}
			}
			
		} while (!performed_action);
		Destroy (indicator);
		current_ap -= 1;
		print ("after ap decrease: " + current_ap);
		menu_showing = false;
		
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
		default:
			break;
		}
		
		if (current_ap > 0) {
			show_menu ();
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
		
		float _t = 0;
		
		while (_t < 1) {
			_t += Time.deltaTime / t;
			transform.position = Vector3.Lerp (start, end, _t);
		}
		transform.position = end;
		if (current_ap > 0) {
			show_menu ();
		}
	}
	
	public virtual void attack_target (Unit u)
	{
		print ("attacking targetting: " + u);
		u._class.hp -= _class.damage;
	}
    #endregion
}