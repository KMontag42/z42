 using UnityEngine;
using System.Collections;
using Pathfinding;

public class NavAgent : MonoBehaviour
{
	 //The point to move to
    public Vector3 targetPosition;
    
    private Seeker seeker;
    private CharacterController controller;
	private Unit unit;
 
    //The calculated path
    public Path path;
    
    //The AI's speed per second
    public float speed = 100;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
 
    public void Start () {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
		unit = GetComponent<Unit>();
        
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        //seeker.StartPath (transform.position,targetPosition, OnPathComplete);
    }
    
    public void OnPathComplete (Path p) {
        Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
    }
 
    public void FixedUpdate () {
		
//		if (Input.GetMouseButtonDown(0)) {
//			RaycastHit hit;
//			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
//				seeker.StartPath(transform.position, hit.point, OnPathComplete);		
//			}
//			
//		}
		
		
        if (unit.path == null) {
            //We have no path to move after yet
            return;
        }
        
        if (currentWaypoint >= unit.path.vectorPath.Count) {
            Debug.Log ("End Of Path Reached");
			currentWaypoint = 0;
			unit.path = null;
            return;
        }
		
        //Direction to the next waypoint
        Vector3 dir = (unit.path.vectorPath[currentWaypoint]-transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
		//iTween.MoveUpdate(gameObject, dir, 1);
        controller.SimpleMove (dir);
		targetPosition = dir;
        
        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance (transform.position,unit.path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }
}
 