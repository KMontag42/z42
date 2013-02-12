using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	
	public List<Unit> Combatants = new List<Unit>();
	public List<Unit> turnOrder = new List<Unit>();
	public int currentUnit = 0;
	public bool InBattle = false;
	
	private static int CompareBySpeed(Unit x, Unit y)
	{		
		if (x == null) {
			if (y == null) {
				return 0;
			}
			else {
				return -1;
			}
		}
		else {
			if (y == null) {
				return 1;
			}
			else {
				//bool eq = x.stats[4].ModValue == y.stats[4].ModValue;
				
				if (x.Stats[4].ModValue > y.Stats[4].ModValue) {
					return 1;
				}
				else if (y.Stats[4].ModValue > x.Stats[4].ModValue) {
					return -1;
				}
				else {
					return 0;
				}
			}
		}
	}
	
	void Start() {}
	void Update() {
	}
	
	// DO USE THIS for Init
	public void Init(List<Unit> c)
	{
		Combatants = c;
		OrderTurns();
		InBattle = true;
		gameObject.AddComponent((typeof (TurnBasedBattle)));
	}
	
	void OrderTurns()
	{
		foreach (Unit u in Combatants) { turnOrder.Add(u); }
		
		turnOrder.Sort(CompareBySpeed);
		
		foreach (Unit u in turnOrder) { Debug.Log(u.name); }
	}
	
	public void EndBattle() {
		Component.Destroy(gameObject.GetComponent((typeof (TurnBasedBattle))));
		InBattle = false;
		Combatants.Clear();
		turnOrder.Clear();
	}
	/**/
}