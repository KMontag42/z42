using UnityEngine;
using System.Collections;

public abstract class PassiveSpell : ISpell
{
	#region Fields
	string _name;
	IUnit[] targets;
	string description;
	int[] effectValue;
	#endregion
	
	#region Properties
	public string Name {
		get { return _name; }
	}
	
	public int Range {
		get { return 0; }
	}
	
	public IUnit[] Targets {
		get { return targets; }
		set { targets = value; }
	}
	
	public SpellType Type {
		get { return SpellType.Passive; }
	}
	
	public string Description {
		get { return description; }
		set { description = value; }
	}
	
	public int[] EffectValue {
		get { return effectValue; }
	}
	
	public int[] Cost {
		get {
			int[] z = {0,0,0,0};
			return z;
		}
	}
	#endregion
	
	#region Methods
	public abstract void Cast(IUnit[] target);
	#endregion
}

