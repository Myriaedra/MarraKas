using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFleurScript : MonoBehaviour {

    public FleurScript fleurScript;
	int whichParticleSpawn = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Disappear()
    {
        fleurScript.Disappear();
    }
    public void Reappear()
    {
        fleurScript.Reappear();
		whichParticleSpawn = 0;
    }
    public void DestroyFlower()
    {
        fleurScript.DestroyFlower();
    }
	public void SpawnParticleOnTheWay(){
		whichParticleSpawn++;
		fleurScript.SpawnParticleOnTheWay (whichParticleSpawn);
	}
}
