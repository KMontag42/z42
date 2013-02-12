using UnityEngine;
using System.Collections;

public abstract class ActiveSpell : ISpell
{
//	#region Fields
//	string _name;
//	int range;
//	SpellType type;
//	string description;
//	int[] cost;
//	int[] effectValue;
//	#endregion
	
	#region Properties
	abstract public string Name {
		get;
	}
	
	abstract public int Range {
		get;
	}
	
	abstract public SpellType Type {
		get;
	}
	
	abstract public string Description {
		get;
	}
	
	abstract public int[] Cost {
		get;
	}
	
	abstract public int[] EffectValue {
		get;
	}
	#endregion
	
	#region Methods
	public abstract void Cast(IUnit[] target);
	#endregion
}

