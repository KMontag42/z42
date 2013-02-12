using UnityEngine;
using System.Collections;

public interface ITalentTree {

	string Name {
		get;
		set;
	}
	ITalent[] Talents {
		get;
		set;
	}
	
	void LevelTalent(ITalent t);
	void ApplyEffects();
}
