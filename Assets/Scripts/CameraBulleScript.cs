using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBulleScript : MonoBehaviour {

	Camera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		cam.fieldOfView = Camera.main.fieldOfView;
	}
}
