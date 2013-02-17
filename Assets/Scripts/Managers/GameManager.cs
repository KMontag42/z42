using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
private
	List<Unit> players = new List<Unit>();
	BattleManager bm;
	Vector3 spawn_position = new Vector3(0,2,0);
	
	public void spawn_player()
	{
		GameObject new_player = Network.Instantiate(Resources.Load("Prefabs/player"),spawn_position,Quaternion.identity, 0) as GameObject;
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
	
	public void OnServerInitialized()
	{
		spawn_player();	
	}
	
	public void OnConnectedToServer()
	{
		spawn_player();	
	}
	
	public void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	
	public void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Debug.Log("Clean up a bit after server quit");
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
		Application.LoadLevel(Application.loadedLevel);
	}
}
