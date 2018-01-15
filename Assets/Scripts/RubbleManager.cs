using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleManager : MonoBehaviour {
	public List<Transform> skSpots;
	public List<GameObject> skeletons;
	int skNeeded;
	bool digged;
	public bool readyToDig;
	public float limitY;

	public Vector3 originalPosition;

	public PlayerController player;

	// Use this for initialization
	void Start () {
		skNeeded = skSpots.Count;
		originalPosition = transform.position;
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

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Skeleton") 
		{
			skeletons.Add (other.gameObject);
			if (skeletons.Count >= skNeeded) 
			{
				readyToDig = true;
			}
		}
	}

	public IEnumerator RubbleClear()
	{
		//Disappear in the ground
		while (transform.position.y > limitY) 
		{
			float xDif = Random.Range (-0.2f, 0.2f);
			float zDif = Random.Range (-0.2f, 0.2f);
			transform.position = new Vector3 (originalPosition.x+xDif, transform.position.y - 0.01f, originalPosition.z+zDif);
			yield return null;
		}

		//Gives control back to the player
		player.PlayerControl(true);
	}
}
