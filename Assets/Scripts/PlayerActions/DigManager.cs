using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigManager : MonoBehaviour {
	private bool digInput;
	public ParticleSystem digFX;
	public Canvas partPreview;
	PartsReference pRef;
	public PlayerController player;
	public Animator anim;

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
	}

	void OnTriggerStay(Collider other)
	{
		float distance = Vector3.Distance (other.transform.position, transform.position);
        //SPOT------------------------------------------------------------------------------------------------
		if (other.tag == "Spot" && digInput && distance<3f) 
		{
			StartCoroutine(Dig (other.transform.GetComponent<SpotManager> ()));
			PlayerController.controlsAble = false;
			digInput = false;
		}
        //RUBBLE----------------------------------------------------------------------------------------------
		else if (other.tag == "Rubble" && digInput && distance<6f) 
		{
			RubbleManager rubMan = other.gameObject.GetComponent<RubbleManager> ();
			if (rubMan.readyToDig)
			{
				rubMan.StartCoroutine ("RubbleClear");
				PlayerController.controlsAble = false;
				anim.SetTrigger ("DigTrigger");
			}
			digInput = false;
		}
        else if(other.tag == "Baril" && digInput && distance < 2f)
        {
            Rigidbody rbBaril = other.GetComponent<Rigidbody>();
            rbBaril.freezeRotation = false;
            rbBaril.isKinematic = false;
            rbBaril.AddForce (new Vector3(0, 50, 0));
        }
        else if(other.tag == "Pebble" && digInput && distance < 2f)
        {
            Rigidbody rbPebble = other.GetComponent<Rigidbody>();
            rbPebble.isKinematic = false;
            rbPebble.AddForce(-transform.forward * 150 + transform.up * 100);
        }
	}

	IEnumerator Dig(SpotManager diggedSpot) 
	{
		anim.SetTrigger ("DigTrigger");
		Debug.Log ("You digged out the " + diggedSpot.type + " number " + diggedSpot.part);
		Camera.main.GetComponent<InventoryManager> ().playerInventory.AddItem (diggedSpot.type, diggedSpot.part);
		ParticleSystem instDigFX = Instantiate (digFX, diggedSpot.transform.position, Quaternion.identity); //Instantiate FX
		Destroy(instDigFX, 2f);

		yield return new WaitForSeconds (2f);
		anim.SetTrigger ("DigOverTrigger");
		PartPreview (diggedSpot.type, diggedSpot.part); //Instantiate canvas with preview of the sk part
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
	}
}

