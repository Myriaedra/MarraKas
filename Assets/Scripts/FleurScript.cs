using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleurScript : MonoBehaviour {

    public Transform[] waypoints;
    int actualWaypoint = -1;
    public Animator meshAnim;
    bool disappearing = false;
    public ParticleSystem part;
	Vector3 positionBeforeDisappear;
	public GameObject particleBurst;

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
		else if(Vector3.Distance(PlayerController.pc.transform.position, transform.position) < 7f && !disappearing &&  actualWaypoint >= waypoints.Length - 1){
            meshAnim.SetTrigger("Destroy");
        }

    }

    public void Disappear()
    {
		positionBeforeDisappear = transform.position;
        part.Pause();
        actualWaypoint++;
    }

	public void SpawnParticleOnTheWay (int where){
		transform.position = Vector3.Lerp (positionBeforeDisappear, waypoints [actualWaypoint].position, where / 15f);
		print (where);
		GameObject partBurst = Instantiate (particleBurst, transform.position, Quaternion.identity);
		Destroy (partBurst, 3.0f);
	}

    public void Reappear()
    {
		print ("prout");
        part.Play();
        disappearing = false;
		transform.position = waypoints[actualWaypoint].position;
    }

    public void DestroyFlower()
    {
        Destroy(gameObject);
    }
}
