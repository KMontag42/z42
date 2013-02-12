using UnityEngine;
using System.Collections;

public interface ISpell {

	string Name {
		get;
	}
	
	int Range {
		get;
	}
	
	SpellType Type {
		get;
	}
	
	string Description {
		get;
	}
	// duration and strength
	int[] EffectValue {
		get;
	}
	
	int[] Cost {
		get;
	}
	
	void Cast(IUnit[] target);
}
