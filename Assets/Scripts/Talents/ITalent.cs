using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ITalent {
	
	string Name {
		get;
		set;
	}
	Dictionary<ISpell, IEffect> ModifiedSpells {
		get;
		set;
	}
	Dictionary<IStat, int> ModifiedStats {
		get;
		set;
	}
	// current and max level
	int[] LevelInfo {
		get;
		set;
	}
	// levels needed to take the talent
	int[] LevelReq {
		get;
		set;
	}
	
	void LevelUp();
}

