using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.UI;

public class BarkManagement : MonoBehaviour {

    [HideInInspector]
    public List<Transform> spotsDetected = new List<Transform>();
    [Space(5)]
    public PlayerController playerController;
    [Space(5)]
    public float showRange;
    [Space(5)]
    public GameObject triggerFlower;
    Transform flowerEmitterTransform;
    [Space(5)]
    public GameObject littleFlowerPartPrefab;
    [Space(5)]
    public GameObject barkPartPrefab;
    [Space(5)]
    public Text barkToInteractText;



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
            Transform actualSpot = other.GetComponent<Transform>();
            spotsDetected.Add(actualSpot);
            print("you're in my list !!!");
        }
    }//new spot in list

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Spot")
        {
            Transform otherRenderer = other.GetComponent<Transform>();
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
            //UI BARK TO INTERACT
            if(!barkToInteractText.enabled && PlayerController.controlsAble)
                barkToInteractText.enabled = true;
            else if(!PlayerController.controlsAble)
                barkToInteractText.enabled = false;

            //OUTLINE CASE BY CASE------------------------------
            //flowerEmitter
            if (hit.collider.CompareTag("FlowerEmitter") && hit.collider.GetComponent<Renderer>()!= rendererInSight)
            {
                OutlineOff();
                rendererInSight = hit.collider.GetComponent<Renderer>();
                OutlineOn();
                whatIsInSight = "FlowerEmitter";
            }
            //SkSpawner
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
            if (barkToInteractText.enabled)
                barkToInteractText.enabled = false;
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
                Vector3 spawnPosition = transform.position + new Vector3(0, -0.8f, 0);
                GameObject barkPart = Instantiate(barkPartPrefab, spawnPosition, Quaternion.identity);
                Destroy(barkPart, 4);
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
            Vector3 spawnPosition = transform.position + new Vector3(0, -0.8f, 0);
            GameObject barkPart = Instantiate(barkPartPrefab, spawnPosition, Quaternion.identity);
            Destroy(barkPart, 4);

            for (int i = 0; i < spotsDetected.Count; i++)
            {
				if (spotsDetected [i] != null) 
				{
					if (Vector3.Distance (spotsDetected [i].transform.position, playerController.transform.position) < showRange) {
                        GameObject flowerPart = Instantiate(littleFlowerPartPrefab, spotsDetected[i].position, Quaternion.identity);
                        Destroy(flowerPart, 4);
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
