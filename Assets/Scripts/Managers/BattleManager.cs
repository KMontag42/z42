using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
		
	public List<Unit> combatants = new List<Unit>();
	public bool in_battle = false;
	public List<PlayerBattle> battles = new List<PlayerBattle>();
	public PlayerBattle current_battle;
	public List<Unit> player_one_units = new List<Unit>();
	public List<Unit> player_two_units = new List<Unit>();
	
	
	public GameManager gm;
	int current_unit_index = 0;
	Rect ap_box = new Rect(50, 50, 150, 75);
	Rect team_1_hp_area = new Rect(Screen.width - 300, 50, 250, 75);
	Rect team_2_hp_area = new Rect(Screen.width - 300, 150, 250, 75);
	
	Rect full_hp_box = new Rect(Screen.width - 300, 50, 250, 50);
	Rect current_hp_box = new Rect(Screen.width - 300, 50, 250, 50);
	
	void Start() {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		combatants.Clear();
	}
	
	void Update() {
		if (combatants.Count > 0) {
			foreach (Unit c in combatants) {
				if (c._class.hp <= 0)
				{
					combatants.Remove(c);
					
					if (c.team == 1)
						player_one_units.Remove(c);
					else if (c.team == 2)
						player_two_units.Remove(c);
					
					Destroy(c.gameObject);
					print (combatants.Count);
				}
			}
			
			if (player_one_units.Count == 0) {
				gm.winner = 2;
				end_battle();	
			}
			
			if (player_two_units.Count == 0) {
				gm.winner = 1;
				end_battle();
			}
		}
	}
	
	// DO USE THIS for Init
	public void init(List<Unit> c)
	{
		combatants = c;
		foreach (Unit _c in combatants) {
			if (_c.team == 1)
				player_one_units.Add(_c);
			else if (_c.team == 2)
				player_two_units.Add(_c);
		}
		in_battle = true;
		if (Network.isServer)
			networkView.RPC("start_player_battle", RPCMode.AllBuffered);
		battles.Add(current_battle);
	}
	
	[RPC]
	public void set_current_battle() {
		if (Network.isServer) {
			GameObject t = Network.Instantiate(Resources.Load("Prefabs/PlayerBattle"), Vector3.zero, Quaternion.identity, 0) as GameObject;
			current_battle = t.GetComponent<PlayerBattle>();
			current_battle.init();
		} else {
			GameObject t = GameObject.Find("PlayerBattle(Clone)") as GameObject;
			current_battle = t.GetComponent<PlayerBattle>();
			current_battle.init();
		}
	}
	
	[RPC]
	public void start_player_battle() {
		if (Network.isServer) {
			networkView.RPC("set_current_battle", RPCMode.AllBuffered);
		}
	}
	
	public void end_battle() {
		in_battle = false;
		combatants.Clear();
		battles.Remove(current_battle);
		current_battle = null;
		GUIOverlay.hideTooltip();
		Application.LoadLevel("win_scene");
	}
	
	public void OnGUI() 
	{
		if (current_battle != null){
			GUILayout.BeginArea(ap_box);
			for (int i = 0; i < current_battle.current_player.current_ap; i++) {
				GUILayout.Box("AP");	
			}
			GUILayout.EndArea();
			
			GUI.Box(team_1_hp_area, "");
			GUILayout.BeginArea(team_1_hp_area);
			foreach (Unit u in player_one_units) {
				GUILayout.Box("Team 1: "+u.name+" HP: "+u._class.hp);
			}
			GUILayout.EndArea();
			GUI.Box(team_2_hp_area, "");
			GUILayout.BeginArea(team_2_hp_area);
			foreach (Unit u in player_two_units) {
				GUILayout.Box("Team 2: "+u.name+" HP: "+u._class.hp);
			}
			GUILayout.EndArea();
		}
	}
}