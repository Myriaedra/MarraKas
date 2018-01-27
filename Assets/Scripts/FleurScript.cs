using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleurScript : MonoBehaviour {

    public Transform[] waypoints;
    int actualWaypoint = 0;
    public Animator meshAnim;
    bool disappearing = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(PlayerController.pc.transform.position, transform.position) < 5f && !disappearing)
        {
            disappearing = true;
            print("hey");
            meshAnim.SetTrigger("Disappearing");
        }
	}

    public void ChangeWaypoint()
    {
        actualWaypoint++;
        transform.position = waypoints[actualWaypoint].position;
    }
}
