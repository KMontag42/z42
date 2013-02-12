using UnityEngine;
using System.Collections;

public interface IItem {

	string Name {
		get;
		set;
	}
	// need a way to represent item effect...
	IUnit Owner {
		get;
		set;
	}
	bool IsBound {
		get;
		set;
	}
	bool IsCrafted {
		get;
		set;
	}
	int Level {
		get;
		set;
	}
	int LevelReq {
		get;
		set;
	}
	
	void Use();
	void Destroy();
	void Drop();
	void Pickup();
	void Trade(IUnit t);
}

