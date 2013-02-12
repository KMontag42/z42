using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellManager {
	
	Dictionary<string, ISpell> spells = new Dictionary<string, ISpell>();
	Dictionary<string, int> cooldowns = new Dictionary<string, int>();
	
	#region Spell Handlers
	public void AddSpell(ISpell s) {
		if (s.Name != null) {
			if (!spells.ContainsKey(s.Name)) {
				spells.Add(s.Name,s);
			}
		}
	}
	
	public ISpell GetSpell(string n) {
		return spells[n];
	}
	
	public ISpell[] GetSpells() {
		ISpell[] returnme = new ISpell[spells.Count];
		int index = 0;
		foreach (ISpell s in spells.Values) {
			returnme[index] = s;
			index++;
		}
		return returnme;
	}
	
	public void RemoveSpell(string s) {
		if (spells.ContainsKey(s)) {
			spells.Remove(s);
		}
	}
	
	public void CastSpell(string s, IUnit[] t) {
		if (spells.ContainsKey(s) && !cooldowns.ContainsKey(s)) {
			ISpell _s = spells[s];
			_s.Cast(t);
			if (_s.Cost[2] >= 1) {
				cooldowns.Add(s, _s.Cost[2]);
			}
		}
	}
	
	
	#endregion
	
	#region Cooldown Handlers
	// to be ran each turn to handle cooldowns
	// decreases each one by 1, and if there is a 0, then removes it
	public void CheckCooldowns() {
		if (cooldowns.Count >= 1)
		{	
			foreach (string i in cooldowns.Keys) {
				cooldowns[i] -= 1;
				if (cooldowns[i] == 0) {
					cooldowns.Remove(i);
				}
			}
		}
	}
	#endregion
	
	public SpellManager(ISpell[] s) {
		Debug.Log("contructor spell length"+s.Length);
		for(int i = 0; i < s.Length; i++) {
			AddSpell(s[i]);
		}
		Debug.Log("spells after contructor"+spells.Count);
	}
}
