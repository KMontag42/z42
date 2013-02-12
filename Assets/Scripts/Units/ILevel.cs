using UnityEngine;
using System.Collections;

public interface ILevel {
	
	/*
	int[] vitalup = new int[3];
	int[] statup = new int[4];
	int talentpoints = 1;
	ISpell[] spellUp = new ISpell[2];
	IAttack[] attackUp = new IAttack[1];
	// cur lvl, cur exp, needed exp, tp
	int[] expinfo = new int[4];

	public string Name {
		get { return "Level 1 - Solider"; }
	}
	public int[] VitalUp {
		get { return vitalup; }
		set { vitalup = value; }
	}
	public int[] StatUp {
		get { return statup; }
		set { statup = value; }
	}
	public int TalentPoints {
		get { return talentpoints; }
		set { talentpoints = value; }
	}
	public IAttack[] AttackUp {
		get { return attackUp; }
		set { attackUp = value; }
	}
	
	public int[] ExpInfo {
		get { return expinfo; }
		set { expinfo = value; }
	}
	
	public SoliderL1() {
		vitalup[0] = 500;
		vitalup[1] = 100;
		vitalup[2] = 10;
		
		statup[0] = 10;
		statup[1] = 10;
		statup[2] = 10;
		statup[3] = 5;
		
		attackUp[0] = new Melee();
		
		expinfo[0] = 1;
		expinfo[1] = 0;
		expinfo[2] = 100;
		expinfo[3] = 1;
	}
	*/
	
	string Name {
		get;
	}
	// HP, AT, KI
	int[] VitalUp {
		get;
		set;
	}
	// STR, AGI, INT, Move
	int[] StatUp {
		get;
		set;
	}
	int TalentPoints {
		get;
		set;
	}
	IAttack[] AttackUp {
		get;
		set;
	}
	// vital, stat, tp, spell, attack, exp needed
	int[] ExpInfo {
		get;
		set;
	}
}

