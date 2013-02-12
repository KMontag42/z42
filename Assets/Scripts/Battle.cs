using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle : MonoBehaviour {
	/*
	public List<Tile> battleTiles = new List<Tile>();
	public List<Unit> enemies = new List<Unit>();
	public List<Unit> player = new List<Unit>();
	
	private List<Unit> turnOrder = new List<Unit>();
	private Dictionary<Unit,string> actionsTaken = new Dictionary<Unit, string>();
	
	private static int CompareBySpeed(Unit x, Unit y)
	{
		if (x == null)
		{
			if (y == null)
			{
				return 0;
			}
			else
			{
				return -1;
			}
		}
		else
		{
			if (y == null)
			{
				return 1;
			}
			else
			{
				bool x_y = x.stats[4].ModValue > y.stats[4].ModValue;
				bool y_x = y.stats[4].ModValue > x.stats[4].ModValue;
				//bool eq = x.stats[4].ModValue == y.stats[4].ModValue;
				
				if (x_y)
				{
					return 1;
				}
				else if (y_x)
				{
					return -1;
				}
				else
				{
					return 0;
				}
			}
		}
	}
	
	
	// DO USE THIS for Init
	void Init(List<Tile> bt, List<Unit> e, List<Unit> p)
	{
		battleTiles = bt;
		enemies = e;
		player = p;
		OrderTurns();
	}
	
	// DONT use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OrderTurns()
	{
		if (turnOrder == null)
		{
			turnOrder.AddRange(enemies);
			turnOrder.AddRange(player);
		}
		
		turnOrder.Sort(CompareBySpeed);
		
		Debug.Log(turnOrder.ToString());
	}
	
	void SetActiveUnit(Unit u)
	{
		// make it this unit's turn
	}
	
	void DeactivateUnit(Unit u)
	{
		// end this unit's turn
	}
	
	void RecordAction(Unit u, string a)
	{
		actionsTaken.Add(u,a);
	}
	*/
}
