using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellSound : MonoBehaviour {
	AudioSource aS;
	public float baseVolume;
	public AudioClip ringSFX;
	// Use this for initialization
	void Start () 
	{
		aS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FirstRing()
	{
		aS.volume = baseVolume;
		//aS.pitch = Random.Range (0.95f, 1.05f);
		aS.PlayOneShot (ringSFX);
	}

	void Ring()
	{
		aS.volume -= 0.1f;
		aS.pitch = 1f;
		aS.PlayOneShot (ringSFX);
	}

	void RingUp()
	{
		aS.volume -= 0.1f;
		aS.pitch = 0.999f;
		aS.PlayOneShot (ringSFX);
	}
}
