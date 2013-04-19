using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
private
	List<Unit> players = new List<Unit>();
	BattleManager bm;
	
	public void spawn_player(Vector3 spawn_position)
	{
		GameObject new_player = GameObject.Instantiate(Resources.Load("Prefabs/player"),spawn_position,Quaternion.identity) as GameObject;
		players.Add (new_player.GetComponent<Unit>());
	}
	
	public void start_battle()
	{
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
	
	public void OnGUI () {
		GUILayout.BeginVertical();
		if (GUILayout.Button ("Spawn Player"))
		{
			spawn_player (GameObject.Find("spawn_one").transform.position);
			spawn_player (GameObject.Find("spawn_two").transform.position);
		}
		if (GUILayout.Button ("Start Battle"))
		{
			start_battle();	
		}
	}
}
