using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerBattle : MonoBehaviour
{
	public List<Unit> players = new List<Unit> ();
	public Unit current_player;
	
	public int index = 0;
	public int selected_unit_material = 0;
	public int previous_unit_material;
	public BattleManager bm;
	
	public void Start() {}
	
	public void Update() {}
	
	public void init ()
	{
		networkView.RPC("set_battle_manager", RPCMode.Server);
		networkView.RPC("set_players", RPCMode.Server);
		networkView.RPC ("OrderTurns", RPCMode.Server);
		networkView.RPC ("StartBattle", RPCMode.Server);
		Debug.Log(bm);
	}
	
	[RPC]
	public void set_battle_manager() {
		bm = GameObject.Find("BattleManager").GetComponent<BattleManager>();
	}
	
	[RPC]
	public void set_players() {
		players = bm.combatants;	
	}
	
	private static int CompareBySpeed (Unit x, Unit y)
	{
		if (x == null) {
			if (y == null) {
				return 0;
			} else {
				return -1;
			}
		} else {
			if (y == null) {
				return 1;
			} else {
				bool x_y = x._class.speed > y._class.speed;
				bool y_x = y._class.speed > x._class.speed;
				//bool eq = x.stats[4].ModValue == y.stats[4].ModValue;
				
				if (x_y) {
					return -1;
				} else if (y_x) {
					return 1;
				} else {
					return 0;
				}
			}
		}
	}
	
	[RPC]	
	private void OrderTurns()
	{
		players.Sort(CompareBySpeed);
	}
	
	[RPC]
	private void StartBattle()
	{
		if (Network.isServer) {
			//networkView.RPC("set_index", RPCMode.Server, 0);
			//networkView.RPC("set_current_player", RPCMode.Server, index);
			index = 0;
			current_player = players[index];
			StartTurn(ref current_player);
		}
	}
	
	[RPC]
	private void set_index(int i) {
		index = i;
	}
	
	[RPC]
	private void set_current_player(int i) {
		try {
			current_player = players[i];
		} catch (ArgumentOutOfRangeException e) {
			print (e + " from: " + Network.player);	
		}
	}
	
	public void StartTurn(ref Unit p)
	{
		if (Network.isServer) {
			if (p.renderer.material.color.r == 255)
				previous_unit_material = 1;
			else if (p.renderer.material.color.b == 255)
				previous_unit_material = 2;
			p.networkView.RPC("set_current_ap", RPCMode.AllBuffered, p._class.action_points);
			p.networkView.RPC("set_material", RPCMode.AllBuffered, selected_unit_material);
			p.networkView.RPC ("show_team_frame_rpc", p.owner);
			Debug.Log("Start turn AP: " + p.current_ap);
			add_player_end_turn_listener();
			p.networkView.RPC("start_turn", p.owner);
		}
	}
	
	public void add_player_end_turn_listener() {
		try {
			current_player.TurnOver += new EventHandler(PlayerEndTurn);	
			print ("listener added");
		} catch (NullReferenceException e) {
			print (e + " from: " + Network.player);
		}
	}
	
	public void PlayerEndTurn(object sender, EventArgs e)
	{
		print ("player end turn");
		current_player.TurnOver -= PlayerEndTurn;
		if (Network.isServer) {
			current_player.networkView.RPC("set_material", RPCMode.AllBuffered, current_player.team);
			index = (index + 1) % players.Count;
			current_player = players[index];
			
			StartTurn(ref current_player);
		}
	}
	
}

