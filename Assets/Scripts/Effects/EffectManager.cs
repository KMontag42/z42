using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager
{
	Dictionary<IEffect, int> effects = new Dictionary<IEffect, int>();
	IUnit owner;
	
	public EffectManager(IUnit o) {
		owner = o;
	}
	
	public void AddEffect(IEffect e, int d) {
		if (!effects.ContainsKey(e))
			effects.Add(e, d);
	}
	
	public void ClearEffects() {
		effects.Clear();
	}
	
	public bool HasEffect(IEffect e) {
		return effects.ContainsKey(e);
	}
	
	public void ManageEffects() {
		foreach (IEffect e in effects.Keys) {
			manageEffect(e);
		}
	}
	
	void manageEffect(IEffect e) {
		e.Apply(owner);
		effects[e] -= 1;
		if (effects[e] == 0)
			RemoveEffect(e);
	}
	
	public void RemoveEffect(IEffect e) {
		if(effects.ContainsKey(e))
			effects.Remove(e);	
	}
}

