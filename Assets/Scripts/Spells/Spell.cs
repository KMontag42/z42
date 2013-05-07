using UnityEngine;
using System.Collections;

public class Spell
{
	public enum TYPE
	{
		SELF,
		ENEMY,
		ALLY
	}
	
	public int effect_damage;
	public int spell_range;
	public int spell_area;
	public TYPE type;
	public string effect;
	
	// defaults
	public Spell() {}
	
	public Spell(int e_d, int s_r, int s_a, TYPE t)
	{
		effect_damage = e_d;
		spell_range = s_r;
		spell_area = s_a;
		type = t;
	}
	
	public virtual void perform_spell(Unit caster, Unit target){}
}

