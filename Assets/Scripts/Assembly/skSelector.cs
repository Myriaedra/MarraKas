using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skSelector : MonoBehaviour {
	skSpawner skSP;
	InventoryClass playerInventory;
	PartsReference pRef;

	Vector3 selectPosition;

	GameObject[] currentParts = new GameObject[3];

	int[] selectedParts = new int[3];
	int selectedType;
	bool snaped;


	// Use this for initialization
	void Start () 
	{
		skSP = GetComponent<skSpawner> ();
		pRef = Camera.main.GetComponent<PartsReference> ();
		playerInventory = Camera.main.GetComponent<InventoryManager> ().playerInventory;
		InitCurrentParts ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Select type and parts
		CheckNavigation();

		//Validate
		if (Input.GetButtonDown("Jump"))
			CreateSkeleton();

	}

	//Navigates between head, torso and leg, and differant parts in he inventory
	void CheckNavigation()
	{
	///////////Type
		if (Input.GetAxis("Vertical") > 0.2f && !snaped) 
		{
			selectedType--;
			print ("type = " + nfmod(selectedType, 3));
			snaped = true;
		}

		if (Input.GetAxis("Vertical") < -0.2f && !snaped) 
		{
			selectedType++;
			print ("type = " + nfmod(selectedType, 3));
			snaped = true;
		}

	////////Selected Part OLD
	/*
		if (Input.GetAxis("Horizontal") > 0.2f && !snaped) 
		{
			int modType = selectedType % 3;
			switch (modType) 
			{
			case 0:
				selectedHead++;
				if (playerInventory.heads.Count != 0) {
					print ("head n°" + nfmod(selectedHead, playerInventory.heads.Count));
				}
				break;
			case 1:
				selectedTorso++;
				if (playerInventory.torsos.Count != 0) {
					print ("torso n°" + nfmod(selectedTorso, playerInventory.torsos.Count));
				}
				break;
			case 2:
				selectedLeg++;
				if (playerInventory.legs.Count != 0) {
					print ("leg n°" + nfmod(selectedLeg, playerInventory.legs.Count));
				}
				break;
			}

			snaped = true;
		}

		if (Input.GetAxis("Horizontal") < -0.2f && !snaped) 
		{
			int modType = selectedType % 3;
			switch (modType) 
			{
			case 0:
				selectedHead--;
				if (playerInventory.heads.Count != 0) {
					print ("head n°" + nfmod(selectedHead, playerInventory.heads.Count));
				}
				break;
			case 1:
				selectedTorso--;
				if (playerInventory.torsos.Count != 0) {
					print ("torso n°" + nfmod(selectedTorso, playerInventory.torsos.Count));
				}
				break;
			case 2:
				selectedLeg--;
				if (playerInventory.legs.Count != 0) {
					print ("leg n°" + nfmod(selectedLeg, playerInventory.legs.Count));
				}
				break;
			}

			snaped = true;
		}
		*/
		if (Input.GetAxis("Horizontal") > 0.2f && !snaped) 
		{
			int modType = nfmod(selectedType, 3);
			selectedParts [modType]++;
			UpdateVisualisation(modType, playerInventory.GetIDFromReference(modType, nfmod(selectedParts[modType], playerInventory.GetArrayLength(modType))));

			snaped = true;
		}

		if (Input.GetAxis("Horizontal") < -0.2f && !snaped) 
		{
			int modType = selectedType % 3;
			selectedParts [modType]--;
			UpdateVisualisation(modType, playerInventory.GetIDFromReference(modType, nfmod(selectedParts[modType], playerInventory.GetArrayLength(modType))));

			snaped = true;

		}
	/////////Reset input
		if ((Input.GetAxis ("Vertical") < 0.2f && Input.GetAxis ("Vertical") > -0.2f) 
			&& (Input.GetAxis ("Horizontal") < 0.2f && Input.GetAxis ("Horizontal") > -0.2f)) 
		{
			snaped = false;
		}	
	}

	void UpdateVisualisation(int type, int part)
	{
		Destroy (currentParts [type]);
		currentParts[type] = Instantiate (pRef.GetPrefabFromReference (type, playerInventory.GetIDFromReference(type, nfmod(selectedParts[type], playerInventory.GetArrayLength(type)))), selectPosition, Quaternion.identity);
	}

	//Spawn a skeleton from the chosen parts if none of them is void
	void CreateSkeleton()
	{
		if (playerInventory.heads.Count > 0 && playerInventory.torsos.Count > 0	&& playerInventory.legs.Count > 0)
		{
			skSP.SpawnFromParts (currentParts[0], currentParts[1], currentParts[2]);
		} else {
			print ("no can do");
		}
	}

	void InitCurrentParts()
	{
		if (playerInventory.heads.Count > 0) 
		{
			currentParts[0] = Instantiate (pRef.GetPrefabFromReference (0, playerInventory.heads [0]), selectPosition, Quaternion.identity);
		}
		if (playerInventory.torsos.Count > 0) 
		{
			currentParts[1] = Instantiate (pRef.GetPrefabFromReference (1, playerInventory.torsos [0]), selectPosition, Quaternion.identity);
		}
		if (playerInventory.legs.Count > 0) 
		{
			currentParts[2] =Instantiate (pRef.GetPrefabFromReference (2, playerInventory.legs [0]), selectPosition, Quaternion.identity);
		}
	}

	//Better modulo
	int nfmod(float a,float b)
	{
		return Mathf.RoundToInt(a - b * Mathf.Floor(a / b));
	}
}
