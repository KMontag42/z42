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
	public TYPE type;
	
	public Spell(int e_d, int s_r, TYPE t)
	{
		effect_damage = e_d;
		spell_range = s_r;
		type = t;
	}
	
	public virtual void perform_spell(Unit target){}
}

