using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFleurScript : MonoBehaviour {

    public FleurScript fleurScript;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeWaypoint()
    {
        fleurScript.ChangeWaypoint();
    }
}
