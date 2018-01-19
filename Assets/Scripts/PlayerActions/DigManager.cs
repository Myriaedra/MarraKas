using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigManager : MonoBehaviour {
	private bool digInput;
	public ParticleSystem digFX;
	public Canvas partPreview;
	PartsReference pRef;
	public BarkManagement barkMan;
	public PlayerController player;

	void Start()
	{
		pRef = Camera.main.GetComponent<PartsReference> ();
	}

	void Update()
	{
		if (Input.GetButtonDown ("Dig") && !digInput && PlayerController.controlsAble) {
			digInput = true;	
		} else if (digInput) {
			digInput = false;
		}

		/*if (Input.GetKeyDown ("x")) { //Test for preview
			PartPreview (0, 1);
		}*/
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Spot" && digInput) 
		{
			StartCoroutine(Dig (other.transform.GetComponent<SpotManager> ()));
			PlayerController.controlsAble = false;
			digInput = false;
		}

		if (other.tag == "Rubble" && digInput) 
		{
			RubbleManager rubMan = other.gameObject.GetComponent<RubbleManager> ();
			if (rubMan.readyToDig) 
			{
				rubMan.StartCoroutine ("RubbleClear");
				PlayerController.controlsAble = false;
			}
			digInput = false;
		}
	}

	IEnumerator Dig(SpotManager diggedSpot) 
	{
		Debug.Log ("You digged out the " + diggedSpot.type + " number " + diggedSpot.part);
		Camera.main.GetComponent<InventoryManager> ().playerInventory.AddItem (diggedSpot.type, diggedSpot.part);
		//print (diggedSpot.gameObject.GetComponent<Animator> ());
		//barkMan.spotsDetected.Remove(barkMan.spotsDetected.Find(diggedSpot.gameObject.GetComponent<Animator>()));//Add the sk part in the inventory
		ParticleSystem instDigFX = Instantiate (digFX, diggedSpot.transform.position, Quaternion.identity); //Instantiate FX
		Destroy(instDigFX, 2f);

		yield return new WaitForSeconds (2f);


		PartPreview (diggedSpot.type, diggedSpot.part); //Instantiate canvas with preview of the sk part
		//Destroy (fx, 1.5f);
		Destroy (diggedSpot.transform.gameObject);
	}

	public void PartPreview(int type, int part) //Spawn canvas with preview
	{
		Canvas newCanvas = Instantiate (partPreview);
		newCanvas.worldCamera = Camera.main;
		GameObject foundPart;
		switch (type) 
		{
			case 0:
				foundPart = Instantiate (pRef.GetPrefabFromReference (type, part), newCanvas.GetComponentInChildren<RectTransform> ()); //spawn right sk part
				foundPart.transform.localPosition = new Vector3 (0, -353, -53);
				foundPart.transform.localRotation = Quaternion.Euler(new Vector3 (-5, -16, 0));
				foundPart.transform.localScale = new Vector3 (200, 200, 200);
				break;

			case 1 :
				foundPart = Instantiate (pRef.GetPrefabFromReference (type, part), newCanvas.GetComponentInChildren<RectTransform> ()); //spawn right sk part
				foundPart.transform.localPosition = new Vector3 (0, -127, -80);
				foundPart.transform.localScale = new Vector3 (100, 100, 100);
				break;

			case 2 :
				foundPart = Instantiate (pRef.GetPrefabFromReference (type, part), newCanvas.GetComponentInChildren<RectTransform> ()); //spawn right sk part
				foundPart.transform.localPosition = new Vector3 (0, -56, -84);
			foundPart.transform.localRotation = Quaternion.Euler(new Vector3 (0, 196, 0));
				foundPart.transform.localScale = new Vector3 (100, 100, 100);
				break;

		}
		//foundPart.AddComponent<Rotator>();
	}
}

