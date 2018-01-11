using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class skSelector : MonoBehaviour {
	skSpawner skSP;
	InventoryClass playerInventory;
	PartsReference pRef;
	public PlayerController player;

	bool isActivated;
	public CinemachineVirtualCamera assemblyView;

	public GameObject confettiFX;

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
		selectPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActivated) {
			//Select type and parts
			CheckNavigation ();

			//Validate
			if (Input.GetButtonDown ("Jump"))
				CreateSkeleton ();
		} 
		else 
		{
			if (Input.GetKeyDown ("a")) {
				BeginAssembly ();
			}
		}
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
		currentParts[type] = Instantiate (pRef.GetPrefabFromReference (type, playerInventory.GetIDFromReference(type, nfmod(selectedParts[type], playerInventory.GetArrayLength(type)))), selectPosition, transform.rotation);
	}

	//Spawn a skeleton from the chosen parts if none of them is void
	void CreateSkeleton()
	{
		if (playerInventory.heads.Count > 0 && playerInventory.torsos.Count > 0	&& playerInventory.legs.Count > 0)
		{
			skSP.SpawnFromParts (currentParts[0], currentParts[1], currentParts[2]);
			playerInventory.RemoveItem (0, playerInventory.GetIDFromReference (0, nfmod (selectedParts [0], playerInventory.GetArrayLength (0))));
			playerInventory.RemoveItem (1, playerInventory.GetIDFromReference (1, nfmod (selectedParts [1], playerInventory.GetArrayLength (1))));
			playerInventory.RemoveItem (2, playerInventory.GetIDFromReference (2, nfmod (selectedParts [2], playerInventory.GetArrayLength (2))));
		} else {
			print ("no can do");
		}
		GameObject conf = Instantiate (confettiFX, transform.position, Quaternion.Euler(new Vector3 (-90,0,0)));
		Destroy (conf, 6f);
		EndAssembly ();
	}

	public void InitCurrentParts()
	{
		if (playerInventory.heads.Count > 0) 
		{
			currentParts[0] = Instantiate (pRef.GetPrefabFromReference (0, playerInventory.heads [0]), selectPosition, transform.rotation);
		}
		if (playerInventory.torsos.Count > 0) 
		{
			currentParts[1] = Instantiate (pRef.GetPrefabFromReference (1, playerInventory.torsos [0]), selectPosition, transform.rotation);
		}
		if (playerInventory.legs.Count > 0) 
		{
			currentParts[2] =Instantiate (pRef.GetPrefabFromReference (2, playerInventory.legs [0]), selectPosition, transform.rotation);
		}
	}

	public void BeginAssembly()
	{
		InitCurrentParts ();
		assemblyView.enabled = true;
		isActivated = true;
		player.PlayerControl (false);
	}

	public void EndAssembly()
	{
		assemblyView.enabled = false;
		isActivated = false;
		player.PlayerControl (true);
	}

	//Better modulo
	int nfmod(float a,float b)
	{
		return Mathf.RoundToInt(a - b * Mathf.Floor(a / b));
	}
}
