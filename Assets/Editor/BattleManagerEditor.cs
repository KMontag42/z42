using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(BattleManager))]
class BattleManagerEditor : Editor {
	BattleManager bm;

	public override void OnInspectorGUI() 
	{
		bm = GameObject.Find("BattleManager").GetComponent("BattleManager") as BattleManager;	
	    if(GUILayout.Button("Start Battle"))
	    {
			List<Unit> player_units = new List<Unit>();
			
			foreach(GameObject unit in GameObject.FindGameObjectsWithTag("Player"))
			{
			    player_units.Add(unit.GetComponent("Unit") as Unit);
			}
			
			bm.init(player_units);
		}
      
  	}
}