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

	//Prefabs
	public GameObject confettiFX;

	Vector3 selectPosition;
	public Transform mementoPositionMarker;
	Vector3 mementoPosition;

	GameObject[] currentParts = new GameObject[3];
	GameObject currentMementoMesh;
	Memento currentMemento;

	int[] selectedParts = new int[3];
	int selectedMemento;
	int selectedType;
	bool snaped;

	public GameObject uiCanvas;
	UInterface_Assembly ui;

	//Son
	AudioSource aS;
	public AudioClip deniedSFX;
	public AudioClip switchTypeSFX;
	public AudioClip switchPartSFX;
	public AudioClip creationSFX;


	// Use this for initialization
	void Start () 
	{
		skSP = GetComponent<skSpawner> ();
		ui = uiCanvas.GetComponent<UInterface_Assembly> ();
		pRef = Camera.main.GetComponent<PartsReference> ();
		playerInventory = Camera.main.GetComponent<InventoryManager> ().playerInventory;
		aS = GetComponent<AudioSource> ();
		selectPosition = transform.position;
		mementoPosition = mementoPositionMarker.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActivated) 
		{
			//Select type and parts
			CheckNavigation ();

			//Validate
			if (Input.GetButtonDown ("Jump"))
				CreateSkeleton ();

			if (Input.GetButtonDown ("Cancel")) 
			{
				DestroyCurrentParts ();
				StopAllCoroutines ();
				EndAssembly ();
				PlayerController.controlsAble = true;
			}
		} 
	}




 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void CheckNavigation()
	{
	///////////Type
		if (Input.GetAxis("Vertical") > 0.2f && !snaped) 
		{
			selectedType--;
			print ("type = " + nfmod(selectedType, 4));
			ui.UpdateArrows(nfmod(selectedType, 4), nfmod(selectedType+1, 4));
			snaped = true;
			aS.pitch = 1f;
			aS.PlayOneShot (switchTypeSFX);
		}

		if (Input.GetAxis("Vertical") < -0.2f && !snaped) 
		{
			selectedType++;
			print ("type = " + nfmod(selectedType, 4));
			ui.UpdateArrows(nfmod(selectedType, 4), nfmod(selectedType-1, 4));
			snaped = true;
			aS.pitch = 1f;
			aS.PlayOneShot (switchTypeSFX);
		}

		if (Input.GetAxis("Horizontal") > 0.2f && !snaped) 
		{
			int modType = nfmod(selectedType, 4);
			if (modType < 3) {
				selectedParts [modType]++;
				UpdateVisualisation (modType, playerInventory.GetIDFromReference (modType, nfmod (selectedParts [modType], playerInventory.GetArrayLength (modType))));
			} 
			else 
			{
				selectedMemento++;
				currentMemento = playerInventory.mementos[nfmod(selectedMemento, playerInventory.mementos.Count)];
				ui.UpdateName (currentMemento);
				UpdateVisualisation (modType, currentMemento.ID);
			}
			aS.pitch = 1f;
			aS.PlayOneShot (switchPartSFX);
			snaped = true;
		}

		if (Input.GetAxis("Horizontal") < -0.2f && !snaped) 
		{
			int modType = nfmod(selectedType, 4);
			if (modType < 3) {
				selectedParts [modType]--;
				UpdateVisualisation (modType, playerInventory.GetIDFromReference (modType, nfmod (selectedParts [modType], playerInventory.GetArrayLength (modType))));
			} 
			else 
			{
				selectedMemento--;
				currentMemento = playerInventory.mementos[nfmod(selectedMemento, playerInventory.mementos.Count)];
				ui.UpdateName (currentMemento);
				UpdateVisualisation (modType, currentMemento.ID);
			}
			aS.pitch = 0.999f;
			aS.PlayOneShot (switchPartSFX);
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
		if (type < 3) 
		{
			Destroy (currentParts [type]);
			currentParts [type] = Instantiate (pRef.GetPrefabFromReference (type, part), selectPosition, transform.rotation);
		} 
		else 
		{
			Destroy (currentMementoMesh);
			currentMementoMesh = Instantiate(pRef.GetPrefabFromReference(3, part), mementoPosition, transform.rotation);
		}
	}





	//Spawn a skeleton from the chosen parts if none of them is void
	void CreateSkeleton()
	{
		if (playerInventory.heads.Count > 0 && playerInventory.torsos.Count > 0	&& playerInventory.legs.Count > 0 && playerInventory.mementos.Count > 0)
		{
			if (currentMemento.ID == 0) 
			{
				GameObject.Find ("Tuto").GetComponent<TutoManager> ().skCreated = true;
			}

			skSP.SpawnFromParts (currentParts[0], currentParts[1], currentParts[2], currentMemento);
			playerInventory.RemoveItem (0, playerInventory.GetIDFromReference (0, nfmod (selectedParts [0], playerInventory.GetArrayLength (0))));
			playerInventory.RemoveItem (1, playerInventory.GetIDFromReference (1, nfmod (selectedParts [1], playerInventory.GetArrayLength (1))));
			playerInventory.RemoveItem (2, playerInventory.GetIDFromReference (2, nfmod (selectedParts [2], playerInventory.GetArrayLength (2))));
			playerInventory.RemoveItem (currentMemento);
			Destroy (currentMementoMesh);
			aS.PlayOneShot (creationSFX);
			GameObject conf = Instantiate (confettiFX, transform.position, Quaternion.Euler(new Vector3 (-90,0,0)));
			Destroy (conf, 6f);
			EndAssembly ();



		} else {
			aS.PlayOneShot (deniedSFX);
		}	
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
		if (playerInventory.mementos.Count > 0) {
			currentMemento = playerInventory.mementos [0];
			currentMementoMesh = Instantiate (pRef.GetPrefabFromReference (3, currentMemento.ID), mementoPosition, transform.rotation);
		}

		selectedType = 0;
	}

	public void ResetCurrentParts()
	{
		for (int i = 0; i < 3; i++) 
		{
			currentParts[i] = null;
		}
		currentMemento = null;
	}

	public void DestroyCurrentParts()
	{
		for (int i = 0; i < 3; i++) {
			Destroy (currentParts [i]);
		}
		Destroy(currentMementoMesh); 
	}




	public IEnumerator BeginAssembly()
	{
		InitCurrentParts ();
		assemblyView.enabled = true;
		isActivated = true;
		PlayerController.controlsAble = false;
		player.SetRenderer(false);
		yield return new WaitForSeconds (1);
		uiCanvas.SetActive(true);
		ui.InitArrows (currentMemento);

	}

	public void EndAssembly()
	{
		assemblyView.enabled = false;
		uiCanvas.SetActive(false);
		ResetCurrentParts ();
		isActivated = false;
		player.SetRenderer(true);
		//PlayerController.controlsAble = true;
	}






	//Better modulo
	int nfmod(float a,float b)
	{
		return Mathf.RoundToInt(a - b * Mathf.Floor(a / b));
	}
}
