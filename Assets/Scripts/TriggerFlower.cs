using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFlower : MonoBehaviour {

    public GameObject spottedParticle;
    ParticleSystem part;
    Rigidbody rb;
    public Vector3 wantedVelocity;
    public float timerLife;
    bool emittingStopped = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        timerLife = Time.time;
        part = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(wantedVelocity.x, 0, wantedVelocity.z);
        var sh = part.shape;
        sh.scale += new Vector3(0.25f, 0.06f, 0);
        if (Time.time - timerLife > 5 && !emittingStopped)
        {
            emittingStopped = true;
            part.Stop();
        }
        if (Time.time - timerLife > 7)
            Destroy(gameObject);
	}

    void OnTriggerEnter(Collider other)
    {
        print("coucou");
        if (other.CompareTag("Spot"))
        {
            Instantiate(spottedParticle, other.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
    }
}
