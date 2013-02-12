using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// think about making static classes for each stat...? look into it
public class Stat {
	
	public string name;
	private int baseValue;
	private int modifiedValue;
	private Dictionary<string,int> modifiers = new Dictionary<string, int>();
	
	public int BaseValue
	{
		get { return baseValue; }
		set { baseValue = value; }
	}
	public int ModValue
	{
		get { return modifiedValue; }
		set { modifiedValue = value; }
	}
	public Dictionary<string, int> Modifiers
	{
		get { return modifiers; }
		set { modifiers = value; }
	}
	
	public Stat()
	{
	}
	
	public Stat(string n, int b)
	{
		name = n;
		baseValue = b;
	}
	
	public Stat(string n, int b, Dictionary<string, int> m)
	{
		name = n;
		baseValue = b;
		modifiers = m;
	}
	
	void calcModValue()
	{
		modifiedValue = baseValue;
		foreach (int i in modifiers.Values)
		{
			modifiedValue += i;
		}
	}
}
