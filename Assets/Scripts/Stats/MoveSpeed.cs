using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveSpeed : IStat {

	int baseValue;
	int modValue;
	Dictionary<string, int> modifiers = new Dictionary<string, int>();
	
	public MoveSpeed(int i)
	{
		baseValue = i;
		modValue = 0;
	}
	
	public string Name {
		get { return "Movement"; }
	}
	public int BaseValue {
		get { return baseValue; }
	}
	public int ModValue {
		get { return baseValue + modValue; }
	}
	public Dictionary<string, int> Modifiers {
		get { return modifiers; }
		set { modifiers = value; }
	}
	
	public int calcModValue() {
		int r = 0;
		foreach (int i in modifiers.Values) {
			r += i;
		}
		
		return r;
	}
	
	public void increaseBaseValue(int n) {
		baseValue += n;
	}
}
