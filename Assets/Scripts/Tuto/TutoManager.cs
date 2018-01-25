using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TutoManager : MonoBehaviour {
	public List<GameObject> neededParts;

	bool gotLegTorso;
	bool gotEverything;
	public bool skCreated;
	bool tutoEnded;

	//Timelines
	int timelineID;
	public TimelineAsset[] timelines;
	public PlayableDirector pD;


	//Dialogues
	public skDialogueUI UIDialogueText;
	public float dialogueID;
	int previousID;
	public string[] tutoDialogues;

	// Use this for initialization
	void Start () {
		previousID = -1;
		dialogueID = -1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (previousID != Mathf.FloorToInt(dialogueID) )
		{
			previousID = Mathf.FloorToInt(dialogueID);
			UIDialogueText.StartDisplaying (tutoDialogues [Mathf.FloorToInt(dialogueID)]);
		}

		if (skCreated && timelineID == 2) 
		{
			StartCoroutine ("Cinematic");
		}
	}

	public void CheckTutoPart(GameObject part)
	{
		neededParts.Remove (part);
		if (neededParts.Count <= 2 && !gotLegTorso) {
			gotLegTorso = true;
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
			}
		}
	}

	IEnumerator Cinematic()
	{
		PlayerController.controlsAble = false;
		pD.Play (timelines [timelineID]);
		timelineID++;
		while (pD.state == PlayState.Playing) 
		{
			yield return null;	
		}
		PlayerController.controlsAble = true;
	}
}
