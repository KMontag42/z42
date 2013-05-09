using UnityEngine;
using System.Collections;

public class Protect : Effect {

	public Protect() : base() {
		type = Effect.TYPE.DAMAGE_MOD;
	}
	
	public Protect(int d, int e_d, Effect.TYPE t) : base(d, e_d, t) {
		type = Effect.TYPE.DAMAGE_MOD;
	}
	
	public override void apply_effect(Unit caster, Unit target) {
		target.sharing_dmg = true;
		target.sharing_dmg_with = caster;
		target.sharing_percent = effect_damage;
		
		update_effect();
	}
}
