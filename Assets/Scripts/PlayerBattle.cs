using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerBattle
{
	
	public
	List<Unit> players = new List<Unit> ();
	
	private
	Unit current_player;
	int index;
	

	public PlayerBattle (List<Unit> p)
	{
		players = p;
		OrderTurns();
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
				bool x_y = x.speed > y.speed;
				bool y_x = y.speed > x.speed;
				//bool eq = x.stats[4].ModValue == y.stats[4].ModValue;
				
				if (x_y) {
					return 1;
				} else if (y_x) {
					return -1;
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
		p.current_ap = p._class.action_points;
		p.TurnOver += new EventHandler(PlayerEndTurn);
		do {
			p.show_menu();
		} while (p.current_ap > 0);
	}
	
	private void PlayerEndTurn(object sender, EventArgs e)
	{
		index++;
		index %= players.Count;
		current_player = players[index];
		StartTurn(ref current_player);
	}
	
	
}

