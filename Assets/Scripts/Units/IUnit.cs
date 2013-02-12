using UnityEngine;
using System.Collections;

// only half of what a player needs, player needs to have PlayerMover as well
public interface IUnit {

	string Name {
		get;
	}
	IUnitClass Class {
		get;
	}
	EffectManager EfManager {
		get;
		set;
	}
	// inventory manager
	
	void TakeDamage(int[] d);
}

