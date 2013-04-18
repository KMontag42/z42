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
		print ("bm init");
		combatants = c;
		print ("combatants set");
		in_battle = true;
		print ("in battle");
		current_battle = new PlayerBattle(combatants);
		battles.Add(current_battle);
		print ("turn base battle added to gameObject");
	}
	
	public void end_battle() {
		in_battle = false;
		combatants.Clear();
	}
	/**/
}