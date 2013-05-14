 using UnityEngine;
using System.Collections;

public class NavAgent : MonoBehaviour
{
	private NavMeshAgent agent;
	private bool isTweening = false;
	//public GameObject player;
	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update ()
	{
		if (agent.autoTraverseOffMeshLink) {
			Debug.Log("is on offmesh link");
			isTweening = true;
			//agent.nextOffMeshLinkData.offMeshLink.activated
			Vector3 startPos = agent.currentOffMeshLinkData.startPos;
			Vector3 endPos = agent.currentOffMeshLinkData.endPos;
			//agent.SetDestination(endPos);
			
			
			//iTween.MoveTo(agent.gameObject, iTween.Hash("time", 2 , "x", endPos.x, "y", endPos.y, "z", endPos.z, "easeType", "easeInOutExpo"));
		}
		if (!isTweening) {
			RaycastHit hit;
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit))
					agent.SetDestination (hit.point);
           
			}
		}
	}
}
 