using UnityEngine;
using System.Collections;

public class TheifSpell : Spell
{
	public TheifSpell()
	{
		effect_damage = 120;
		spell_range = 2;
		spell_area = 1;
		type = Spell.TYPE.SELF;
		effect = "theif_charged";
	}
	
	public TheifSpell(int e_d, int s_r, int s_a, Spell.TYPE t)
	{
		effect_damage = e_d;
		spell_range = s_r;
		spell_area = s_a;
		type = t;	
	}
	
	public override void perform_spell(Unit caster, Unit target)
	{
		if (caster == target)
		{
			caster.boost_dmg = true;
			caster.boost_percent = effect_damage;
			GameObject g = GameObject.Instantiate(Resources.Load("Prefabs/"+effect), caster.transform.position, Quaternion.identity) as GameObject;
			g.transform.parent = caster.transform;
		}
	}
}

