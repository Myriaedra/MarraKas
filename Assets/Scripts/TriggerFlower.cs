using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFlower : MonoBehaviour {

    public GameObject spottedParticle;
    Rigidbody rb;
    public Vector3 wantedVelocity;
    public float timerLife;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        timerLife = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(wantedVelocity.x, 0, wantedVelocity.z);
        transform.localScale += new Vector3(0.1f, 0, 0);
        if(Time.time - timerLife > 10)
        {
            Destroy(gameObject);
        }
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
