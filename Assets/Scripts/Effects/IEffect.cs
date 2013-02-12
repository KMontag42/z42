using UnityEngine;
using System.Collections;

public interface IEffect {

	/*
	 * what to change
	 * how much to change it by
	 * how long to change it for
	 * dispellable
	 */
	
	string Name {
		get;
	}
	int Duration {
		get;
		set;
	}
	EffectType Type {
		get;
	}
	int Strength {
		get;
		set;
	}
	
	void Apply(IUnit u);
}
