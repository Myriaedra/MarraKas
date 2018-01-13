using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;

public class BarkManagement : MonoBehaviour {

    [HideInInspector]
    public List<Animator> spotsDetected = new List<Animator>();
    [Space(5)]
    public PlayerController playerController;
    [Space(5)]
    public float showRange;
    [Space(5)]
    [Header("SpawningSpotDetector")]
    public GameObject triggerFlower;
    Transform flowerEmitterTransform;

    Renderer rendererInSight;
    string whatIsInSight = "Nothing";
	
	// Update is called once per frame
	void Update () {
        CheckWhatIsInFront();

        if (Input.GetButtonDown ("Bark")) 
		{
			Bark();
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Spot")
        {
            Animator actualSpot = other.GetComponent<Animator>();
            spotsDetected.Add(actualSpot);
            print("you're in my list !!!");
        }
    }//new spot in list

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Spot")
        {
            Animator otherRenderer = other.GetComponent<Animator>();
            spotsDetected.Remove(otherRenderer);
            print("not anymore");
        }
    }//spot out of list

    void CheckWhatIsInFront()
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Interactable");
        if (Physics.Raycast(transform.position, transform.forward, out hit, 9, layerMask))
        {
            if (hit.collider.CompareTag("FlowerEmitter") && hit.collider.GetComponent<Renderer>()!= rendererInSight)
            {
                OutlineOff();
                rendererInSight = hit.collider.GetComponent<Renderer>();
                OutlineOn();
                whatIsInSight = "FlowerEmitter";
            }
            else if (hit.collider.CompareTag("SkSpawner") && hit.collider.transform.GetChild(1).GetComponent<Renderer>() != rendererInSight)
            {
                OutlineOff();
                rendererInSight = hit.collider.transform.GetChild(1).GetComponent<Renderer>();
                OutlineOn();
                whatIsInSight = "SkSpawner";
            }
        }
        else
        {
            if (whatIsInSight != "Nothing")
            {
                OutlineOff();
                whatIsInSight = "Nothing";
            }
        }
    }

    /// <summary>
    /// This thing does things.
    /// </summary>
    public void Bark()
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Interactable");
        if (Physics.Raycast(transform.position, transform.forward, out hit, 9, layerMask))
        {
            if (hit.collider.CompareTag("FlowerEmitter"))
            {
                flowerEmitterTransform = hit.collider.GetComponent<Transform>();
                FlowerEmitterAction();
            }

			//assembly
			if (hit.collider.CompareTag("SkSpawner"))
			{
					skSelector selector = hit.collider.GetComponent<skSelector>();
					selector.BeginAssembly();
			}
        }
        else
        {
            for (int i = 0; i < spotsDetected.Count; i++)
            {
				if (spotsDetected [i] != null) 
				{
					if (Vector3.Distance (spotsDetected [i].transform.position, playerController.transform.position) < showRange) {					
						spotsDetected [i].SetTrigger ("EmissionTrigger");
					}
				}
				else
					spotsDetected.RemoveAt (i);
            }
        }
    }

    public void FlowerEmitterAction()
    {
        GameObject newTriggerFlower = Instantiate(triggerFlower, flowerEmitterTransform.position, flowerEmitterTransform.rotation);
        newTriggerFlower.GetComponent<TriggerFlower>().direction = flowerEmitterTransform.forward;
    }

    void OutlineOff()
    {
        if (rendererInSight != null)
        {
            rendererInSight.material.SetFloat("_OutlineSwitch", 0);
            rendererInSight = null;
        }
    }
    void OutlineOn()
    {
        if (rendererInSight != null)
        {
            rendererInSight.material.SetFloat("_OutlineSwitch", 1);
        }
    }
}
