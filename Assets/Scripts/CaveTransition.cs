using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CaveTransition : MonoBehaviour {
	public AudioMixerSnapshot beachSnp;
	public AudioMixerSnapshot caveSnp;
	public float transitionTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			caveSnp.TransitionTo(transitionTime);
			print ("IN");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			caveSnp.TransitionTo(transitionTime);
			print ("OUT");
		}
	}
}
