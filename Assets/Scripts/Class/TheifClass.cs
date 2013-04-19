using UnityEngine;
using System.Collections;

public class TheifClass : UnitClass
{
	public static int cost = 350;
	public TheifClass()
	{
		speed = 25;
		move_range = 7;
		attack_range = 6;
		action_points = 3;
		hp = 100;
		physical_defence = 5;
		magic_defence = 5;
		damage = 10;
	}
}

