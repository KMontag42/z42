using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    #region Fields
private
	float move_range;
	float attack_range;
	GameObject indicator;
    #endregion
	
	
	
    #region Methods
    // Use this for initialization
	public virtual void Start () {
		move_range = 5;
		attack_range = 3;
	}
	
	// Update is called once per frame
	public virtual void FixedUpdate () {
        
		//unitMenuRect.x = Camera.main.WorldToScreenPoint(transform.position).x;
		//unitMenuRect.y = Camera.main.WorldToScreenPoint(transform.position).y;
	}
	
	public virtual void OnMouseDown()
	{
		ShowMenu();
	}
	
	public void ShowMenu()
	{
		Camera camera_3d = Camera.allCameras[0];
		Vector3 pos = camera_3d.WorldToScreenPoint(transform.position);
		GUIOverlay.showTooltip(this, pos.x, Screen.height - pos.y);
	}
	
	public IEnumerator Move ()
	{
		print ("moving");
		indicator = GameObject.Instantiate(Resources.Load("Prefabs/ground_indicator") as GameObject, transform.position, transform.rotation) as GameObject;
		Vector3 mouse_input = new Vector3(0,0,0);
		Vector3 target_destination = new Vector3(0,0,0);
		bool ready_to_move = false;
		do
		{
			yield return StartCoroutine(GetInput());
			mouse_input = Input.mousePosition;
			
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.allCameras[0].ScreenPointToRay(mouse_input);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast(ray, out hitdist)) {
				target_destination = ray.GetPoint(hitdist);
				print (Vector3.Distance(transform.position, target_destination));
				if (Vector3.Distance(transform.position, target_destination) <= move_range)
				{
					Quaternion targetRotation = Quaternion.LookRotation(target_destination - transform.position);
					transform.rotation = targetRotation;
					ready_to_move = true;	
				}
				
			}
			
		} while (!ready_to_move);
		print ("moved");
		print (target_destination);
		Destroy(indicator);
		
		yield return StartCoroutine(MoveTo (target_destination, 1));
	}
	
	public IEnumerator Attack ()
	{
		print ("attacking");
		indicator = GameObject.Instantiate(Resources.Load("Prefabs/range_indicator") as GameObject, transform.position, transform.rotation) as GameObject;
		Vector3 mouse_input = new Vector3(0,0,0);
		Vector3 target_destination = new Vector3(0,0,0);
		bool ready_to_attack = false;
		do
		{
			yield return StartCoroutine(GetInput());
			mouse_input = Input.mousePosition;
			
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.allCameras[0].ScreenPointToRay(mouse_input);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast(ray, out hitdist)) {
				target_destination = ray.GetPoint(hitdist);
				print (Vector3.Distance(transform.position, target_destination));
				if (Vector3.Distance(transform.position, target_destination) <= attack_range)
				{
					Quaternion targetRotation = Quaternion.LookRotation(target_destination - transform.position);
					transform.rotation = targetRotation;
					ready_to_attack = true;	
				}
				
			}
			
		} while (!ready_to_attack);
		print ("attacked");
		print (target_destination);
		Destroy(indicator);
		
		yield return null;
	}
	
	public IEnumerator Defend ()
	{
		print ("defending");
		indicator = GameObject.Instantiate(Resources.Load("Prefabs/ground_indicator") as GameObject, transform.position, transform.rotation) as GameObject;
		Vector3 mouse_input = new Vector3(0,0,0);
		Vector3 target_destination = new Vector3(0,0,0);
		bool ready_to_defend = false;
		do
		{
			yield return StartCoroutine(GetInput());
			mouse_input = Input.mousePosition;
			
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.allCameras[0].ScreenPointToRay(mouse_input);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast(ray, out hitdist)) {
				target_destination = ray.GetPoint(hitdist);
				print (Vector3.Distance(transform.position, target_destination));
				Quaternion targetRotation = Quaternion.LookRotation(target_destination - transform.position);
				transform.rotation = targetRotation;
				ready_to_defend = true;
				
			}
			
		} while (!ready_to_defend);
		print ("defended");
		print (target_destination);
		Destroy(indicator);
		
		yield return null;
	}
	
	public IEnumerator Spell ()
	{
		print ("casting spell");
		// change based on type of spell
		indicator = GameObject.Instantiate(Resources.Load("Prefabs/range_indicator") as GameObject, transform.position, transform.rotation) as GameObject;
		Vector3 mouse_input = new Vector3(0,0,0);
		Vector3 target_destination = new Vector3(0,0,0);
		bool ready_to_cast = false;
		do
		{
			yield return StartCoroutine(GetInput());
			mouse_input = Input.mousePosition;
			
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.allCameras[0].ScreenPointToRay(mouse_input);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast(ray, out hitdist)) {
				target_destination = ray.GetPoint(hitdist);
				print (Vector3.Distance(transform.position, target_destination));
				if (Vector3.Distance(transform.position, target_destination) <= attack_range)
				{
					Quaternion targetRotation = Quaternion.LookRotation(target_destination - transform.position);
					transform.rotation = targetRotation;
					ready_to_cast = true;	
				}
				
			}
			
		} while (!ready_to_cast);
		print ("spell casted");
		print (target_destination);
		Destroy(indicator);
		
		yield return null;
	}
	
	public IEnumerator Wait ()
	{
		print ("waiting");
		indicator = GameObject.Instantiate(Resources.Load("Prefabs/ground_indicator") as GameObject, transform.position, transform.rotation) as GameObject;
		Vector3 mouse_input = new Vector3(0,0,0);
		Vector3 target_destination = new Vector3(0,0,0);
		bool ready_to_wait = false;
		do
		{
			yield return StartCoroutine(GetInput());
			mouse_input = Input.mousePosition;
			
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.allCameras[0].ScreenPointToRay(mouse_input);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast(ray, out hitdist)) {
				target_destination = ray.GetPoint(hitdist);
				print (Vector3.Distance(transform.position, target_destination));
				Quaternion targetRotation = Quaternion.LookRotation(target_destination - transform.position);
				transform.rotation = targetRotation;
				ready_to_wait = true;
			}
			
		} while (!ready_to_wait);
		print ("waited");
		print (target_destination);
		Destroy(indicator);
		
		yield return null;
	}
	
	public IEnumerator GetInput()
	{
		bool input_recieved = false;
		
		while (!input_recieved) 
		{
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast(ray, out hitdist)) {
				transform.LookAt(ray.GetPoint(hitdist), Vector3.up);
				indicator.transform.LookAt(ray.GetPoint(hitdist), Vector3.up);
				if (Input.GetMouseButton(0))
				{
					input_recieved = true;
					yield return null;	
				}
				yield return null;
			}
			yield return null;
		}
	}
	
    public virtual IEnumerator MoveTo (Vector3 pos, float t)
	{
		Vector3 start = transform.position;
		Vector3 end = pos;
		
		float _t = 0;
		
		while (_t < 1)
		{
			_t += Time.deltaTime / t;
			transform.position = Vector3.Lerp(start, end, _t);
			yield return null;
		}
		transform.position = end;
	}
    #endregion
	
	#region Event Listeners
	void OnEnable()
	{
		Messenger<int[]>.AddListener("attacked",OnAttacked);
	}
	
	void OnDisable()
	{
		Messenger<int[]>.RemoveListener("attacked",OnAttacked);
	}
	
	void OnAttacked(int[] d) {
		
	}
	
	#endregion
}