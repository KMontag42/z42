using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {

	public override void OnInspectorGUI() 
	{
		
		if (GUILayout.Button("Spawn Player"))
		{
			(target as GameManager).spawn_player();	
		}
		
	    if(GUILayout.Button("Start Battle"))
	    {
			Debug.Log ("begin init");
			(target as GameManager).start_battle();
		}
		
		if (GUILayout.Button("Print Units"))
		{
			(target as GameManager).print_players();	
		}
      
  	}
}
