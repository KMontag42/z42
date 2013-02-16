using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
private
	List<Unit> players = new List<Unit>();
	BattleManager bm;
	
	public void spawn_player()
	{
		GameObject new_player = GameObject.Instantiate(Resources.Load("Prefabs/player")) as GameObject;
		players.Add(new_player.GetComponent("Unit") as Unit);
	}
	
	public void start_battle()
	{
		print ("battle starting");
		bm.init(players);	
	}
	
	public void print_players()
	{
		foreach (Unit u in players)
		{
			print (u);	
		}
	}
	
	// Use this for initialization
	void Start () {
		bm = GameObject.Find("BattleManager").GetComponent("BattleManager") as BattleManager;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
