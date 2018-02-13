using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPreviewScript : MonoBehaviour {

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Cancel")) //Quit
			{
				Destroy(gameObject);
				PlayerController.controlsAble = true;
			}
	}
}
