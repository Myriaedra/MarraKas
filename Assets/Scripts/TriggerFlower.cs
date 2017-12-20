using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFlower : MonoBehaviour {

    public GameObject spottedParticle;
    ParticleSystem part;
    BoxCollider col;
    public Vector3 direction;
    public float timerLife;
    bool emittingStopped = false;
    float speed = 0.2f;

	// Use this for initialization
	void Start () {
        timerLife = Time.time;
        part = GetComponent<ParticleSystem>();
        col = GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += direction * speed;
        var sh = part.shape;
        sh.scale += new Vector3(0.25f, 0.1f, 0);
        col.size = sh.scale;
        transform.position -= transform.up*0.03f;
        if (Time.time - timerLife > 6 && !emittingStopped)
        {
            emittingStopped = true;
            part.Stop();
        }
        if (Time.time - timerLife > 8)
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
