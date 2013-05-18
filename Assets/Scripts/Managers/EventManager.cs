using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
	
	public delegate void UnitsReadyHandler ();
	public event UnitsReadyHandler UnitsReadyEvent;
	
	public void OnUnitsReady() {
		if (UnitsReadyEvent != null)
			UnitsReadyEvent();
	}
	// Use this for initialization
	void Start (){
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update (){}
	
	
}

