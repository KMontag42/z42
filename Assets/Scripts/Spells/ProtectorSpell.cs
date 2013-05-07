using UnityEngine;
using System.Collections;

public class ProtectorSpell : Spell
{
	// defaults
	public ProtectorSpell()
	{
		effect_damage = 20;
		spell_range = 10;
		spell_area = 1;
		type = Spell.TYPE.ALLY;
		effect = "Ground Vortex";
	}
	
	public ProtectorSpell(int e_d, int s_r, int s_a, TYPE t)
	{
		effect_damage = e_d;
		spell_range = s_r;
		spell_area = s_a;
		type = t;
		effect = "Ground Vortex";
	}
	
	override public void perform_spell(Unit caster, Unit target)
	{
		Debug.Log("performing protectorspell from "+caster+" on "+target);
		if (!target.sharing_dmg){
			target.sharing_dmg = true;
			target.sharing_dmg_with = caster;
			target.sharing_percent = effect_damage;
			GameObject g = GameObject.Instantiate(Resources.Load("Prefabs/"+effect), target.transform.position, Quaternion.identity) as GameObject;
			g.transform.parent = target.transform;
		}
	}
}

