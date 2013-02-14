using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(BattleManager))]
class BattleManagerEditor : Editor {
	
  public override void OnInspectorGUI() {
		
    if(GUILayout.Button("Test"))
      Debug.Log("It's alive: " + target.name);
      
  }
}