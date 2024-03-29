﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TutoManager : MonoBehaviour {
	public List<GameObject> neededParts;

	public SphereCollider triggerHead;
	public SphereCollider triggerCabin;

	bool gotLegTorso;
	bool gotEverything;
	public bool skCreated;
	bool tutoEnded;
	bool captainReady;

	//Timelines
	int timelineID;
	public TimelineAsset[] timelines;
	public PlayableDirector pD;

	public bool barkTuto;


	//Dialogues
	public skDialogueUI UIDialogueText;
	public float dialogueID;
	int previousID;
	public string[] tutoDialogues;

	Animator boxDialogueAnim;

	//Elements
	public GameObject[] digSpots;
	public GameObject[] mementos;

	// Use this for initialization
	void Start () {
		previousID = -1;
		dialogueID = -1f;
		boxDialogueAnim = GameObject.Find("BoxDialogueContainer").GetComponent<Animator>();

		StoreElements ();
		SetElementsActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (previousID != Mathf.FloorToInt(dialogueID) )
		{
			previousID = Mathf.FloorToInt(dialogueID);
			UIDialogueText.StartDisplaying (tutoDialogues [Mathf.FloorToInt(dialogueID)], new Memento (0, "Captain Tuto", 3, .09f));
		}

		if (skCreated && timelineID == 2) 
		{
			StartCoroutine ("Cinematic");
			triggerCabin.enabled = true;
		}
	}

	public void CheckTutoPart(GameObject part)
	{
		neededParts.Remove (part);
		if (neededParts.Count <= 2 && !gotLegTorso) {
			gotLegTorso = true;
			//transform.GetComponentInChildren<skDialogueManager> ().enabled = true;
			ActivateCollectability (neededParts [0]);
			ActivateCollectability (neededParts [1]);
		} 
		else if (neededParts.Count == 0 && !gotEverything) 
		{
			gotEverything = true;
		}
	}

	public void ActivateCollectability(GameObject part)
	{
		if (part.transform.childCount > 0) {
			part.transform.GetComponentInChildren<BoxCollider> ().enabled = false;
		} else {
			part.transform.GetComponent<BoxCollider> ().enabled = false;
		}
		part.transform.GetComponent<SphereCollider> ().enabled = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			if (!gotLegTorso && timelineID == 0) 
			{
				StartCoroutine ("Cinematic");
			} 

			else if (gotLegTorso && timelineID == 1) 
			{
				StartCoroutine ("Cinematic");
				triggerHead.enabled = false;
			} 

			else if (!tutoEnded && timelineID == 3 && captainReady) 
			{
				StartCoroutine ("Cinematic");
			}
		}

		if (other.gameObject.tag == "Captain") 
		{
			captainReady = true;
		}
	}

	IEnumerator Cinematic()
	{
		PlayerController.controlsAble = false;
		boxDialogueAnim.SetBool("opened", true);
		pD.Play (timelines [timelineID]);
		timelineID++;
		while (pD.state == PlayState.Playing) 
		{
			yield return null;	
		}
		PlayerController.controlsAble = true;
		boxDialogueAnim.SetBool("opened", false);
		UIDialogueText.ClearDisplay ();
		GameObject captain = GameObject.FindGameObjectWithTag ("Captain");

		if (timelineID == 1) 
		{
			//transform.GetComponentInChildren<skDialogueManager> ().enabled = true;
			barkTuto = true;
		}

		if (timelineID == 3) 
		{
			captain.GetComponent<skCaptainBehaviour> ().MoveToRubble ();
			SetElementsActive (true);
		}

		if (timelineID == 4) 
		{
			captain.GetComponent<skCaptainBehaviour> ().enabled = false;

			skBehaviour skBh = captain.AddComponent<skBehaviour> ();
			skBh.SetMemento (new Memento (0, "Captain Tuto", 3, 0.9f));

			skDialogueManager dialogueMan = captain.AddComponent<skDialogueManager>();
			print (GameObject.FindGameObjectWithTag ("SkSpawner").name);
			dialogueMan.mySkSpawner = GameObject.Find("SkSpawner").GetComponent<skSpawner>();
			dialogueMan.mySkBehaviour = skBh;
			skBh.mySkDialogueManager = dialogueMan;
			dialogueMan.SetMemento (new Memento (0, "Captain Tuto", 3, 0.9f));
			dialogueMan.SetUIDialogueText (UIDialogueText);

			captain.tag = "Skeleton";

			tutoEnded = true;

			Destroy (gameObject);

		}
	}

	void StoreElements()
	{
		digSpots = GameObject.FindGameObjectsWithTag ("Spot");
		mementos = GameObject.FindGameObjectsWithTag ("Collectable");
	}

	void SetElementsActive(bool value)
	{
		for (int i = 0; i < digSpots.Length; i++) 
		{
			if (digSpots [i] != null) 
			{
				if (!digSpots [i].GetComponent<TutoPart> ())
					digSpots [i].SetActive (value);
			}
		}

		for (int j = 0; j < mementos.Length; j++) 
		{
			mementos [j].SetActive (value);
		}
	}
}
