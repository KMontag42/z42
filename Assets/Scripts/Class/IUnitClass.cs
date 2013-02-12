using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IUnitClass {

	string Name {
		get;
	}
	// HP, AT, KI
	int[] Vitals {
		get;
		set;
	}
	int[] CurrentVitals {
		get;
		set;
	}
	// STR, AGI, INT, Move, Defence, Dodge, Speed
	IStat[] Stats {
		get;
		set;
	}
	IAttack[] Attacks {
		get;
		set;
	}
	SpellManager SpMan {
		get;
	}
	// talent manager
	// level manager
	
	void Cast(string s, IUnit[] t);
	void Attack(IAttack a, IUnit[] t);
	void GainExp(int e);
	void TakeDamage(int[] d);
	void Init();
}

