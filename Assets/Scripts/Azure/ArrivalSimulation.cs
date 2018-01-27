using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalSimulation : MonoBehaviour {
	bool oneSkSpawned;
	bool seen;

	skSpawner skSp;
	public Transform spawnSpot;
	// Use this for initialization
	void Start () {
		skSp = GetComponent<skSpawner> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!oneSkSpawned && GameObject.FindGameObjectsWithTag ("Skeleton").Length >= 2 && PlayerController.controlsAble && GameObject.Find("Tuto") == null) 
		{
			oneSkSpawned = true;
			StartCoroutine (CheckArrival ());
		}
	}

	IEnumerator CheckArrival()
	{
		yield return new WaitForSeconds (3f);
		while (seen)
		{
			yield return new WaitForSeconds (0.5f);
		}
		//spawn
		print ("it's-a me !");
		skSp.SpawnFromParts (2, 1, 2, new Memento (1, "Mona"), spawnSpot.position);
	}

	void OnBecameInvisible()
	{
		seen = false;
	}

	void OnBecameVisible()
	{
		seen = true;
	}
}
