using UnityEngine;
using System.Collections;

public class Effect : iEffect {
	
	public enum TYPE {
		BUFF,
		DEBUFF,
		DPT,
		DAMAGE_MOD
	}
	
	public int duration;
	public int effect_damage;
	public TYPE type;
	
	// defaults
	public Effect() {
		duration = 	3;
		effect_damage = 10;
		type = TYPE.DPT;
	}
	
	public Effect(int d, int e_d, TYPE t) {
		duration = d;
		effect_damage = e_d;
		type = t;
	}
	
	public virtual void apply_effect(Unit caster, Unit target) {
		if (duration > 0)
			target.take_damage(effect_damage);
		
		update_effect();
	}
	
	public virtual void update_effect() {
		if (duration > 0)
			duration--;
	}
	
	public virtual int get_duration() {
		return duration;
	}
	
	public virtual int get_effect_damage() {
		return effect_damage;	
	}
}
