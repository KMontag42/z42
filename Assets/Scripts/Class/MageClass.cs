using UnityEngine;
using System.Collections;

public class MageClass : UnitClass
{
	public new static int cost = 350;
	public MageClass()
	{
		speed = 10;
		move_range = 5;
		attack_range = 50;
		action_points = 2;
		hp = 100;
		physical_defence = 0;
		magic_defence = 0;
		damage = 25;
	}
}

