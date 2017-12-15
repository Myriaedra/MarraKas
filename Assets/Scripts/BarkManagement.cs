using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkManagement : MonoBehaviour {

    List<Animator> spotsDetected = new List<Animator>();
    public PlayerController playerController;

    public float showRange;
    public float hintRange;

    public GameObject triggerFlower;
    Transform flowerEmitterTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Spot")
        {
            Animator actualSpot = other.GetComponent<Animator>();
            spotsDetected.Add(actualSpot);
            print("you're in my list !!!");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Spot")
        {
            Animator otherRenderer = other.GetComponent<Animator>();
            spotsDetected.Remove(otherRenderer);
            print("not anymore");
        }
    }

    /// <summary>
    /// This thing does things.
    /// </summary>
    public void Bark()
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("BarkInteractable");
        if (Physics.Raycast(transform.position, transform.forward, out hit, 9, layerMask))
        {
            print(hit.collider.name);
            if (hit.collider.CompareTag("FlowerEmitter"))
            {
                flowerEmitterTransform = hit.collider.GetComponent<Transform>();
                FlowerEmitterAction();
            }
        }
        else
        {
            for (int i = 0; i < spotsDetected.Count; i++)
            {
                if (Vector3.Distance(spotsDetected[i].transform.position, playerController.transform.position) < showRange)
                    spotsDetected[i].SetTrigger("EmissionTrigger");
            }
        }
    }

    public void FlowerEmitterAction()
    {
        GameObject newTriggerFlower = Instantiate(triggerFlower, flowerEmitterTransform.position, flowerEmitterTransform.rotation);
        Vector3 olala = flowerEmitterTransform.position - transform.position;
        newTriggerFlower.GetComponent<TriggerFlower>().wantedVelocity = flowerEmitterTransform.forward * 0.3f;
    }
}
