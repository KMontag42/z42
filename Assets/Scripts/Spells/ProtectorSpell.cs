using UnityEngine;
using System.Collections;

public class ProtectorSpell : Spell
{
	// defaults
	public ProtectorSpell()
	{
		effect_damage = 20;
		spell_range = 10;
		spell_area = 3;
		type = Spell.TYPE.ALLY;
		effect = "protector_effect";
	}
	
	public ProtectorSpell(int e_d, int s_r, int s_a, TYPE t)
	{
		effect_damage = e_d;
		spell_range = s_r;
		spell_area = s_a;
		type = t;
		effect = "protector_effect";
	}
	
	override public void perform_spell(Unit caster, Unit target)
	{
		Debug.Log("performing protectorspell from "+caster+" on "+target);
		target.networkView.RPC("request_protector_spell_rpc", RPCMode.Server, effect_damage, effect, 1, caster.special_id);
	}
}

