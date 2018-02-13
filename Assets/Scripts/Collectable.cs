using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
	float amplitude = 0.2f;
	float floatSpeed = 0.7f;
	float collectSpeed = 1.5f;
	float angularSpeed = 200;
	public bool collected = false;
	Transform player;

	public Canvas partPreview;

	public bool memento;
	public bool preview = true;
	public bool floating = true;

	//Iventory
	public int mementoID;
	public string mementoName;
	Memento thisMemento;

	public int type;
	public int part;

	public AudioClip soulSFX;


	// Use this for initialization
	void Start () {
		if (floating)
			StartCoroutine ("Floating");
		
		player = GameObject.FindWithTag ("Player").GetComponent<Transform>();
		thisMemento = new Memento (mementoID, mementoName);
	}
	
	void OnTriggerEnter(Collider other)//TRIGGER ENTER------------------------------------------------------
	{
		if (other.tag == "Player")//CHECK POUR LES COLLECTABLES---------------------------------
		{
				StartCoroutine ("Collected");
		}//-----------------------------------------------------------------------------------
	}

	//Coroutine : oscillate between position+amplitude && position-amplitude on Y axis
	//Uses sine wave
	IEnumerator Floating()
	{
		float t = 0;
		float baseY = transform.position.y;
		while (!collected)
		{
			t += floatSpeed * Time.deltaTime;
			transform.position = new Vector3 (transform.position.x, baseY + Mathf.Sin (t) * amplitude, transform.position.z);

			yield return null;
		}
	}

	//Coroutine : rotate around the player while getting closer
	//Then auto-destroy
	IEnumerator Collected()
	{
		collected = true;
		//get distance from player
		float distance = Vector2.Distance (new Vector2 (player.position.x, player.position.z), new Vector2 (transform.position.x, transform.position.z));
		//get angle
		float angle = 180 + Vector2.Angle(new Vector2 (player.position.x, player.position.z) - new Vector2 (transform.position.x, transform.position.z), new Vector2 (player.position.x, player.position.z) - new Vector2 (0,0));
		while (distance > 0) 
		{
			//Get closer from player and increment rotation angle
			distance -= collectSpeed * Time.deltaTime;
			angle += angularSpeed * Time.deltaTime;

			//Calculate new coordinate relative to player position
			float rad = angle * Mathf.Deg2Rad;
			Vector3 newPos = new Vector3 (Mathf.Cos (rad), 0, Mathf.Sin (rad)) * distance;

			//Add to player position and update own position
			transform.position = player.position + newPos;
				
			yield return null;
		}
		if (memento) {
			Camera.main.GetComponent<InventoryManager> ().playerInventory.AddItem (thisMemento);
			if (preview) {
				PlayerController.controlsAble = false;
				//PartPreview (mementoID);
				PartPreviewOverlay();

			}
		} else {
			Camera.main.GetComponent<InventoryManager> ().playerInventory.AddItem (type, part);
		}


		if (memento) 
		{
			GameObject whisper = new GameObject();
			AudioSource whispSource = whisper.AddComponent<AudioSource> ();
			whispSource.clip = soulSFX;
			whispSource.volume = 0.5f;
			Instantiate (whisper, new Vector3 (0, 0, 0), Quaternion.identity);
			Destroy (whisper, 2f);
		}

		Destroy (gameObject);

	}

	/*public void PartPreview(int ID) //Spawn canvas with preview
	{
		PartsReference pRef = Camera.main.GetComponent<PartsReference> ();
		Canvas newCanvas = Instantiate (partPreview);
		newCanvas.worldCamera = Camera.main;
		GameObject foundPart;
		foundPart = Instantiate (pRef.GetPrefabFromReference (3, mementoID), newCanvas.GetComponentInChildren<RectTransform> ()); //spawn right sk part
		foundPart.transform.localPosition = new Vector3 (0.5f, 1.7f, -13.8f);
		/*foundPart.transform.localRotation = Quaternion.Euler(new Vector3 (-5, -16, 0));*/
		/*foundPart.transform.localScale = new Vector3 (100f, 100f, 100f);
	}*/

	public void PartPreviewOverlay()
	{
		Instantiate (partPreview);
	}
}
