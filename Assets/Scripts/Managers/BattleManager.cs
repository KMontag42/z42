using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
		
	public List<Unit> combatants = new List<Unit>();
	public bool in_battle = false;
	public List<PlayerBattle> battles = new List<PlayerBattle>();
	public PlayerBattle current_battle;
	
private
	int current_unit_index = 0;
	
	void Start() {}
	void Update() {}
	
	// DO USE THIS for Init
	public void init(List<Unit> c)
	{
		combatants = c;
		in_battle = true;
		current_battle = new PlayerBattle(combatants);
		battles.Add(current_battle);
	}
	
	public void end_battle() {
		in_battle = false;
		combatants.Clear();
		battles.Remove(current_battle);
		current_battle = null;
	}
}