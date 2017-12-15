using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsReference : MonoBehaviour {
	public GameObject[] headPrefabs;
	public GameObject[] torsoPrefabs;
	public GameObject[] legPrefabs;

	public GameObject[] mementoPrefabs;

	public GameObject GetPrefabFromReference (int type, int part) //Returns a prefab skeleton part depending on the type (head, leg, torso) and the position in the array
	{
		switch (type)
		{
		case 0 :
			return headPrefabs[part];

		case 1 :
			return torsoPrefabs[part];

		case 2 :
			return legPrefabs[part];
		}

		return new GameObject();
	} 
}
