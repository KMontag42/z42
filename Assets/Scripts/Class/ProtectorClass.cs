using UnityEngine;
using System.Collections;

public class ProtectorClass : UnitClass
{
	public ProtectorClass()
	{
		speed = 10;
		move_range = 5;
		attack_range = 50;
		action_points = 2;
		hp = 100;
		physical_defence = 10;
		magic_defence = 10;
		damage = 100;
		cost = 250;
	}
}

