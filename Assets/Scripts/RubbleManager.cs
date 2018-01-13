using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleManager : MonoBehaviour {
	public List<Transform> skSpots;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Transform GetSpotTransform ()
	{
		if (skSpots.Count > 0) {
			Transform spotTransform = skSpots [0];
			skSpots.RemoveAt (0);
			return spotTransform;
		} else {
			return null;
		}
	}
}
