using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoInputTrigger : MonoBehaviour {
	public Image xImage, yImage, rTImage;

	bool x;
	bool y;
	bool RT;
	bool moved;
	bool barked;
	bool digged;

	TutoManager tutoMan;
	// Use this for initialization
	void Start () 
	{
		tutoMan = GetComponentInParent<TutoManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!moved && (Input.GetAxis ("Horizontal") > 0.2 || Input.GetAxis ("Horizontal") < -0.2 || Input.GetAxis ("Vertical") > 0.2 || Input.GetAxis ("Vertical") < -0.2)) 
		{
			moved = true;
			rTImage.enabled = true;
			RT = true;

		}

		if (RT && Input.GetAxis ("Sprint") > 0.2) 
		{
			RT = false;
			rTImage.enabled = false;
		}

		if (tutoMan.barkTuto && !x && !barked) {
			x = true;
			xImage.enabled = true;
		}
	
		if (x && Input.GetButtonDown("Bark"))
		{
			barked = true;
			x = false;
			xImage.enabled = false;		
		}

		if (y && Input.GetButtonDown("Dig"))
		{
			y = false;
			yImage.enabled = false;
			Destroy (gameObject);
		}
			
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player" && barked) 
		{
			y = true;
			yImage.enabled = true;
		}
	}
}
