using UnityEngine;
using System.Collections;

public class MageSpell : Spell
{

	//defaults
	public MageSpell()
	{
		effect_damage = 20;
		spell_range = 10;
		spell_area = 6;
		type = Spell.TYPE.ENEMY;
		effect = "mage_explosion";
	}
	
	public MageSpell(int e_d, int s_r, int s_a, Spell.TYPE t)
	{
		effect_damage = e_d;
		spell_range = s_r;
		spell_area = s_a;
		type = t;
		effect = "mage_explosion";	
	}
	
	public override void perform_spell(Unit caster, Unit target)
	{
		target.take_damage(effect_damage);
		GameObject g = GameObject.Instantiate(Resources.Load("Prefabs/"+effect), target.transform.position, Quaternion.identity) as GameObject;
	}
}

