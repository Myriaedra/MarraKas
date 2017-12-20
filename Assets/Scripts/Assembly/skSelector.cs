using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skSelector : MonoBehaviour {
	skSpawner skSP;
	InventoryClass playerInventory;

	int selectedHead;
	int selectedTorso;
	int selectedLeg;
	int selectedType;
	bool snaped;


	// Use this for initialization
	void Start () 
	{
		skSP = GetComponent<skSpawner> ();
		playerInventory = Camera.main.GetComponent<InventoryManager> ().playerInventory;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Selected Type
		CheckNavigation();

		//Validate
		if (Input.GetButtonDown("Jump"))
			CreateSkeleton();

	}

	void CheckNavigation()
	{
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

		if ((Input.GetAxis ("Vertical") < 0.2f && Input.GetAxis ("Vertical") > -0.2f) 
			&& (Input.GetAxis ("Horizontal") < 0.2f && Input.GetAxis ("Horizontal") > -0.2f)) 
		{
			snaped = false;
		}

		//Selected Part
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
	}

	void CreateSkeleton()
	{
		if (playerInventory.heads.Count > 0 && playerInventory.torsos.Count > 0	&& playerInventory.legs.Count > 0)
		{
			int headID = nfmod(selectedHead, playerInventory.heads.Count);
			int torsoID = nfmod(selectedTorso, playerInventory.torsos.Count);
			int legID =  nfmod(selectedLeg, playerInventory.legs.Count); 

			skSP.SpawnFromParts (headID, torsoID, legID);
		} else {
			print ("no can do");
		}
	}

	int nfmod(float a,float b)
	{
		return Mathf.RoundToInt(a - b * Mathf.Floor(a / b));
	}
}
