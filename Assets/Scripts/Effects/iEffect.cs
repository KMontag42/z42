using UnityEngine;
using System.Collections;

public interface iEffect {
	
	void apply_effect(Unit caster, Unit target);
	void update_effect();
	int get_duration();
	int get_effect_damage();
	
}
