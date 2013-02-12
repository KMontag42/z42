using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager
{

	#region Fields
	private GameObject[] itemList;
	#endregion
	
	#region Properties
	public GameObject[] ItemList
	{
		get { return itemList; }
		set { itemList = value; }
	}
	#endregion
	
	#region Methods
	public ItemManager()
	{
		itemList = new GameObject[1];
		
	}
	#endregion
}

