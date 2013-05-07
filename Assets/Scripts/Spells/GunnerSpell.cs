using UnityEngine;
using System.Collections;

public class GunnerSpell : Spell
{
	string target_effect = "poison_effect";
	
	// defaults
	public GunnerSpell ()
	{
		effect_damage = 5;
		spell_range = 15;
		spell_area = 1;
		type = Spell.TYPE.ENEMY;
		effect = "gunner_shot";
	}
	
	public GunnerSpell (int e_d, int s_r, int s_a, Spell.TYPE t)
	{
		effect_damage = e_d;
		spell_range = s_r;
		spell_area = s_a;
		type = t;
	}
	
	public override void perform_spell (Unit caster, Unit target)
	{
		target.is_bleeding = true;
		target.bleed_dmg = effect_damage;
		target.bleed_timer = 3;
		GameObject g = GameObject.Instantiate (Resources.Load ("Prefabs/" + effect), caster.transform.position, Quaternion.identity) as GameObject;
		iTween.MoveTo (g, target.transform.position, .2f);
		GameObject p = GameObject.Instantiate(Resources.Load("Prefabs/"+target_effect), target.transform.position, Quaternion.identity) as GameObject;
		p.transform.parent = target.transform;
	}
}

