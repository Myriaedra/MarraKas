using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifiedBark : MonoBehaviour {
	AudioSource aS;
	public AudioClip barkSFX;
	// Use this for initialization
	void Start () {
		aS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	public IEnumerator BarkSound()
	{
		//yield return new WaitForSeconds(0.1f);
		aS.PlayOneShot (barkSFX);
		yield return null;
	}
}
