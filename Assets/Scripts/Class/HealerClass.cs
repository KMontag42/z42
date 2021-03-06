using UnityEngine;
using System.Collections;

public class HealerClass : UnitClass
{
	public new static int cost = 250;
	public HealerClass()
	{
		speed = 5;
		move_range = 3;
		attack_range = 10;
		action_points = 2;
		hp = 100;
		physical_defence = 0;
		magic_defence = 0;
		damage = -10;
		spell = new HealerSpell();
		spell_range = spell.spell_range;
	}
}

