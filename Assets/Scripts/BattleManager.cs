using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	
public	
	List<Unit> combatants = new List<Unit>();
	List<Unit> turn_order = new List<Unit>();
	bool InBattle = false;
private
	int current_unit_index = 0;
	
	public Unit current_unit {
		get { return turn_order[current_unit_index]; }
	}
	
	private static int compre_by_speed(Unit x, Unit y)
	{		
		return 0;
	}
	
	void Start() {}
	void Update() {}
	
	// DO USE THIS for Init
	public void init(List<Unit> c)
	{
		combatants = c;
		OrderTurns();
		InBattle = true;
		gameObject.AddComponent((typeof (TurnBasedBattle)));
	}
	
	void order_turns()
	{
		foreach (Unit u in combatants) { turn_order.Add(u); }
		
		turn_order.Sort(compre_by_speed);
		
		foreach (Unit u in turn_order) { Debug.Log(u.name); }
	}
	
	public void next_unit() {
		if (current_unit_index > turn_order.Count - 1)
		{
			current_unit_index = 0;	
		} else {
			current_unit_index++;	
		}
	}
	
	public void end_battle() {
		Component.Destroy(gameObject.GetComponent((typeof (TurnBasedBattle))));
		InBattle = false;
		combatants.Clear();
		turn_order.Clear();
	}
	/**/
}