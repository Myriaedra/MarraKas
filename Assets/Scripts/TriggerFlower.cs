using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFlower : MonoBehaviour {

    public GameObject spottedParticle;
    Renderer rend;
    ParticleSystem part;
    BoxCollider col;
    public Vector3 direction;
    public float timerLife;
    bool decreaseOpacity = false;
    float speed = 0.2f;

	// Use this for initialization
	void Start () {
        timerLife = Time.time;
        part = GetComponent<ParticleSystem>();
        col = GetComponent<BoxCollider>();
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        /*var sh = part.shape;
        sh.scale += new Vector3(0.25f, 0.1f, 0);
        col.size = sh.scale;*/
        transform.position += direction * speed;
        transform.localScale += new Vector3(0.25f, 0.1f, 0);
        transform.position -= transform.up*0.03f;
        if (Time.time - timerLife > 6 && !decreaseOpacity)
        {
            decreaseOpacity = true;
            //part.Stop();
        }
        if (decreaseOpacity)
        {
            print(rend.material.GetFloat("_visibility"));
            rend.material.SetFloat("_visibility", rend.material.GetFloat("_visibility") - 0.01f);
            if (rend.material.GetFloat("_visibility") <= 0)
                Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spot"))
        {
            Instantiate(spottedParticle, other.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
    }
}
