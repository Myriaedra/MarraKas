using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryClass
{
	//Datas
	public List<int> heads = new List<int>();
	public List<int> torsos = new List<int>();
	public List<int> legs = new List<int>();
	public List<Memento> mementos = new List<Memento>();

	//Operations
	//ADD ITEM
	public void AddItem (int type, int skPart)
	{
		switch (type)
		{
			case 0 :
			heads.Add(skPart);
			break;

			case 1 :
			torsos.Add(skPart);
			break;

			case 2 :
			legs.Add(skPart);
			break;
		}
	}
	public void AddItem (Memento memento)
	{
		mementos.Add(memento);
	}

	//REMOVE ITEM
	public void RemoveItem (int type, int skPart)
	{
		switch (type)
		{
		case 0 :
			heads.Remove(skPart);
			break;

		case 1 :
			torsos.Remove(skPart);
			break;

		case 2 :
			legs.Remove(skPart);
			break;
		}
	}
	public void RemoveItem (Memento memento)
	{
		mementos.Remove(memento);
	}

	public int GetIDFromReference (int type, int pos) //Returns a prefab skeleton part depending on the type (head, leg, torso) and the position in the array
	{
		switch (type)
		{
		case 0 :
			return heads[pos];

		case 1 :
			return torsos[pos];

		case 2 :
			return legs[pos];
		}

		return -1;
	} 

	public int GetArrayLength (int type) //Returns a prefab skeleton part depending on the type (head, leg, torso) and the position in the array
	{
		switch (type)
		{
		case 0 :
			return heads.Count;

		case 1 :
			return torsos.Count;

		case 2 :
			return legs.Count;
		}

		return -1;
	} 

}