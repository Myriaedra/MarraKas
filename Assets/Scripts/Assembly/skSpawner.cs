using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skSpawner : MonoBehaviour {
	PartsReference pRef;
	// Use this for initialization
	void Start () {
		pRef = Camera.main.GetComponent<PartsReference> ();
		//SpawnFromParts (Random.Range(0,3), Random.Range(0,3), Random.Range(0,3));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnFromParts (int headID, int torsoID, int legID)
	{
		GameObject spawnedTorso = Instantiate (pRef.torsoPrefabs [torsoID], new Vector3 (0, 0, 0), Quaternion.identity);
		GameObject spawnedHead = Instantiate(pRef.headPrefabs[headID], spawnedTorso.transform);
		GameObject spawnedLeg = Instantiate(pRef.legPrefabs[legID], spawnedTorso.transform);

		skPartAdder skPA = spawnedTorso.AddComponent<skPartAdder> ();
		skPA.AddLimb (spawnedHead, spawnedTorso);
		skPA.AddLimb (spawnedLeg, spawnedTorso);
	}
}
