using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPreviewScript : MonoBehaviour {
	PlayerController player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("Jump")) //Quit
			{
				Destroy(gameObject);
				player.SetPlayerControl (true);
			}
	}
}
