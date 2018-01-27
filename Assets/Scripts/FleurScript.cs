using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleurScript : MonoBehaviour {

    public Transform[] waypoints;
    int actualWaypoint = -1;
    public Animator meshAnim;
    bool disappearing = false;
    public ParticleSystem part;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(PlayerController.pc.transform.position, transform.position) < 7f && !disappearing && actualWaypoint<waypoints.Length-1)
        {
            disappearing = true;
            meshAnim.SetTrigger("Disappearing");
        }
        else if(Vector3.Distance(PlayerController.pc.transform.position, transform.position) < 7f && actualWaypoint >= waypoints.Length - 1){
            meshAnim.SetTrigger("Destroy");
        }

    }

    public void Disappear()
    {
        part.Pause();
        actualWaypoint++;
        transform.position = waypoints[actualWaypoint].position;
    }

    public void Reappear()
    {
        part.Play();
        disappearing = false;
    }

    public void DestroyFlower()
    {
        Destroy(gameObject);
    }
}
