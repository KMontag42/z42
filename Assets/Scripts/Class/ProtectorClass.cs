using UnityEngine;
using System.Collections;

public class ProtectorClass : UnitClass
{
	public new static int cost = 250;
	public ProtectorClass()
	{
		speed = 10;
		move_range = 5;
		attack_range = 3;
		action_points = 2;
		hp = 100;
		physical_defence = 10;
		magic_defence = 10;
		damage = 5;
		spell = new ProtectorSpell();
		spell_range = spell.spell_range;
	}
}

