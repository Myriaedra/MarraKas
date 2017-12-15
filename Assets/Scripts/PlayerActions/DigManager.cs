using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigManager : MonoBehaviour {
	public GameObject digVisualize;
	private bool digInput;
	public ParticleSystem digFX;
	public Canvas partPreview;
	public PartsReference pRef;

	void Update()
	{
		if (Input.GetButtonDown ("Dig") && !digInput) {
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
			Dig (other.transform.GetComponent<SpotManager> ());
			digInput = false;
		}
	}

	public void Dig(SpotManager diggedSpot) 
	{
		Debug.Log ("You digged out the " + diggedSpot.type + " number " + diggedSpot.part);
		Camera.main.GetComponent<InventoryManager> ().playerInventory.AddItem (diggedSpot.type, diggedSpot.part); //Add the sk part in the inventory
		Instantiate (digFX, diggedSpot.transform.position, Quaternion.identity); //Instantiate FX
		PartPreview (diggedSpot.type, diggedSpot.part); //Instantiate canvas with preview of the sk part
		//Destroy (fx, 1.5f);
		Destroy (diggedSpot.transform.gameObject);
	}

	public void PartPreview(int type, int part) //Spawn canvas with preview
	{
		Canvas newCanvas = Instantiate (partPreview);
		newCanvas.worldCamera = Camera.main;
		Instantiate (pRef.GetPrefabFromReference (type, part), newCanvas.GetComponentInChildren<RectTransform> ()); //spawn right sk part
	}
}

