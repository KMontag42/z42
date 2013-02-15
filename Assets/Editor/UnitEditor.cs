using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Unit))]
class UnitEditor : Editor {	
	public override void OnInspectorGUI() 
	{
		Unit u = target as Unit;
		if (GUILayout.Button("Show Menu"))
		{
			u.show_menu();
		}
	}
}
