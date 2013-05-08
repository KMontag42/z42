using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerBattle
{
	public List<Unit> players = new List<Unit> ();
	public Unit current_player;
	
	private
	int index;
	int selected_unit_material = 0;
	int previous_unit_material;
	BattleManager bm;

	public PlayerBattle (List<Unit> p, BattleManager _bm)
	{
		players = p;
		OrderTurns();
		StartBattle();
		bm = _bm;
		Debug.Log(bm);
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
	
	private void OrderTurns()
	{
		players.Sort(CompareBySpeed);
	}
	
	private void StartBattle()
	{
		index = 0;
		current_player = players[index];
		StartTurn(ref current_player);
	}
	
	private void StartTurn(ref Unit p)
	{
		if (p.renderer.material.color.r == 255)
			previous_unit_material = 1;
		else if (p.renderer.material.color.b == 255)
			previous_unit_material = 2;
		p.networkView.RPC("start_turn", RPCMode.AllBuffered, Network.player.guid);
		p.networkView.RPC("set_material", RPCMode.AllBuffered, selected_unit_material);
		p.networkView.RPC("set_current_ap",RPCMode.AllBuffered, p._class.action_points);
		bm.networkView.RPC("add_player_end_turn_listener", RPCMode.AllBuffered);
		p.networkView.RPC ("show_menu", RPCMode.AllBuffered, current_player.network_id);
		Debug.Log("Start turn AP: " + p.current_ap);
	}
	
	public void PlayerEndTurn(object sender, EventArgs e)
	{
		current_player.TurnOver -= PlayerEndTurn;
		current_player.networkView.RPC("set_material", RPCMode.AllBuffered, previous_unit_material);
		index++;
		index %= players.Count;
		current_player = players[index];
		StartTurn(ref current_player);
	}
	
}

