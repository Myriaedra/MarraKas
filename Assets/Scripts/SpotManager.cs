using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotManager : MonoBehaviour {
	public int type;
	public int part;
	public bool randomInit;

	AudioSource aS;
	public AudioClip[] foundSFXs;

	// Use this for initialization
	void Start () {
		aS = GetComponent<AudioSource> ();
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

	public void SFX()
	{
		aS.PlayOneShot (foundSFXs [Random.Range (0, foundSFXs.Length)]);
	}
}
