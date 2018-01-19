using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManager : MonoBehaviour {
	public List<GameObject> neededParts;

	bool gotLegTorso;
	bool gotEverything;
	bool skCreated;
	bool tutoEnded;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckTutoPart(GameObject part)
	{
		neededParts.Remove (part);
		if (neededParts.Count <= 2 && !gotLegTorso) {
			gotLegTorso = true;
			ActivateCollectability (neededParts [0]);
			ActivateCollectability (neededParts [1]);
		} 
		else if (neededParts.Count == 0 && !gotEverything) 
		{
			gotEverything = true;
		}
	}

	public void ActivateCollectability(GameObject part)
	{
		if (part.transform.childCount > 0) {
			part.transform.GetComponentInChildren<BoxCollider> ().enabled = false;
		} else {
			part.transform.GetComponent<BoxCollider> ().enabled = false;
		}
		part.transform.GetComponent<SphereCollider> ().enabled = true;
	}
}
