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
	Material selected_unit_material;
	Material previous_unit_material;

	public PlayerBattle (List<Unit> p)
	{
		selected_unit_material = Resources.Load("Materials/selected_unit_material") as Material;
		players = p;
		OrderTurns();
		StartBattle();
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
		previous_unit_material = p.renderer.material;
		p.renderer.material = selected_unit_material;
		p.current_ap = p._class.action_points;
		p.TurnOver += new EventHandler(PlayerEndTurn);
		Debug.Log("Start turn AP: " + p.current_ap);
		p.show_menu();
	}
	
	private void PlayerEndTurn(object sender, EventArgs e)
	{
		current_player.TurnOver -= PlayerEndTurn;
		current_player.renderer.material = previous_unit_material;
		index++;
		index %= players.Count;
		current_player = players[index];
		StartTurn(ref current_player);
	}
	
}

