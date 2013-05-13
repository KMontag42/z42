using UnityEngine;
using System.Collections;

public class HealerSpell : Spell
{

	// default
	public HealerSpell()
	{
		effect_damage = 20;
		spell_range = 10;
		spell_area = 4;
		type = Spell.TYPE.ALLY;
		effect = "healing_effect";
	}
	
	public HealerSpell(int e_d, int s_r, int s_a, Spell.TYPE t)
	{
		effect_damage = e_d;
		spell_range = s_r;
		spell_area = s_a;
		type = t;
		effect = "heal_effect";	
	}
	
	public override void perform_spell(Unit caster, Unit target)
	{
		target.networkView.RPC("request_healer_spell_rpc", RPCMode.Server, effect);
	}
}

