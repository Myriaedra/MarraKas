using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotManager : MonoBehaviour {
	public int type;
	public int part;

	// Use this for initialization
	void Start () {
		InitSpot ();
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
