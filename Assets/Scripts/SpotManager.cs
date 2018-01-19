using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotManager : MonoBehaviour {
	public int type;
	public int part;
	public bool randomInit;

	// Use this for initialization
	void Start () {
		if (randomInit) 
		{
			InitSpot ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitSpot()
	{
		type = Random.Range (0, 3); 
		part = Random.Range (0, 3); 
	}
}
