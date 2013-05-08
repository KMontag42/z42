using UnityEngine;
using System.Collections;

public class GunnerClass : UnitClass
{
	public new static int cost = 350;
	public GunnerClass()
	{
		speed = 20;
		move_range = 7;
		attack_range = 75;
		action_points = 2;
		hp = 100;
		physical_defence = 5;
		magic_defence = 5;
		damage = 15;
		spell = new GunnerSpell();
		spell_range = spell.spell_range;
	}
}

