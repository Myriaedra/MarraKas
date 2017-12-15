using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Memento 
{

	public int ID;
	public string name;

	public Memento (int ID, string name)
	{
		this.ID = ID;
		this.name = name;
	}

}
