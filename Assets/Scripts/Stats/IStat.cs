using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IStat {
	
	string Name {
		get;
	}
	int BaseValue {
		get;
	}
	int ModValue {
		get;
	}
	Dictionary<string, int> Modifiers {
		get;
		set;
	}
	
	int calcModValue();
	void increaseBaseValue(int n);
}
