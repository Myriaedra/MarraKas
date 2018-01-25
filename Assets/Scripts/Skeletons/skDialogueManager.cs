﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skDialogueManager : MonoBehaviour {

	public Memento memento;
	skDialogueUI UIDialogueText;

	public string[] spawnDialogues = new string[3];
	public float[] timeSpawnDialogues = new float[3];

	public string[] casualDialogues = new string[3];
	public float[] timeCasualDialogues = new float[3];

	public string[] hintDialogues = new string[3];
	public float[] timeHintDialogues = new float[3];
	public skBehaviour mySkBehaviour;

	float distancePreDialogue = 16;
	float distanceDialogue = 6;
	float actualDistance;
	float timer;
	string playerZone; //(NoDialogue, PreDialogue, Dialogue)
	public string dialogueState = "NoDialogue"; //(NoDialogue, PreDialogue, Dialogue, OutDialogue, InstantDialogue)
	public string dialogueType; //(Spawn, Casual, Hint)



	void Start(){
		StartCoroutine (CheckDistanceToPlayer ());
	} // Start checkDistance coroutine

	void Update(){
		switch(dialogueState){
		    case "PreDialogue": //At the end you can no longer have the Dialogue
                timer -= Time.deltaTime;
				if(timer<=0){
					UIDialogueText.ClearDisplay ();
				    dialogueState = "NoDialogue";
                    mySkBehaviour.MoveToRubble();
                    PlayerController.pc.beingTalkedTo = null;
			    }
			    break;
            case "OutDialogue": // At the end you can have a PreDialogue again when entering in the PreDialogue Zone
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
					UIDialogueText.ClearDisplay ();
                    dialogueState = "NoDialogue";
                    mySkBehaviour.MoveToRubble();
                    PlayerController.pc.beingTalkedTo = null;
                }
                break;
            case "Dialogue": // At the end you can have an event and you have no more outdialogue when you go somewhere else
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
					UIDialogueText.ClearDisplay ();
                    print("here comes an event ?");
                    dialogueState = "NoDialogue";
                    mySkBehaviour.MoveToRubble();
                    PlayerController.pc.beingTalkedTo = null;
                }
                break;
		}
	} // decrease timers and apply events 

	public void ShowDialogueSetTimer(string typeDialogue /*(Spawn, Casual, Hint)*/, int whichDialogue){
		PlayerController.pc.beingTalkedTo = gameObject;
		mySkBehaviour.state = "Talking";
		switch(typeDialogue){
		    case "Spawn":
				UIDialogueText.StartDisplaying (spawnDialogues [whichDialogue]);
                timer = timeSpawnDialogues[whichDialogue];
                if (whichDialogue == 1)
                    mySkBehaviour.Invoke("MoveToRubble", timer);
			    break;
			case "Casual":
				UIDialogueText.StartDisplaying (casualDialogues [whichDialogue]);
                timer = timeCasualDialogues[whichDialogue];
                break;
			case "Hint":
				UIDialogueText.StartDisplaying (hintDialogues [whichDialogue]);
                timer = timeHintDialogues[whichDialogue];
                break;
		}
	} //Said in title

	IEnumerator CheckDistanceToPlayer(){
		while(true){
			yield return new WaitForSeconds (0.5f);
			actualDistance = Vector3.Distance (transform.position, PlayerController.pc.transform.position);

			//QUAND ON EST LOIN DU PNJ------------------------------------------------------------------------------------------
			if(actualDistance>=distancePreDialogue && playerZone!="NoDialogue"){
				playerZone = "NoDialogue";
			}

			//QUAND ON RENTRE DANS LA ZONE POUR SE FAIRE INTERPELLER------------------------------------------------------------
			else if(actualDistance<=distancePreDialogue && actualDistance>distanceDialogue && playerZone!="PreDialogue"){
				//et qu'on était loin du pnj --> PreDialogue
				if(playerZone=="NoDialogue")
                {
                    StartPreDialogue();
                }
				//et qu'on était proche du pnj --> OutDialogue
				else if(playerZone=="Dialogue"){
                    StartOutDialogue();
				}
				playerZone = "PreDialogue";
			}

			//QUAND ON EST PROCHE DU PNJ---------------------------------------------------------------------------------------
			else if(actualDistance<=distanceDialogue && playerZone!="Dialogue")
            {
                StartDialogue();
				playerZone = "Dialogue";
			}
		}
	} //check distance and choose which type of dialogue should be said by the pnj

    void StartPreDialogue()
    {
        if ((PlayerController.pc.beingTalkedTo == null || PlayerController.pc.beingTalkedTo == gameObject) && dialogueState == "NoDialogue")
        {
            if (Random.Range(0, 2) == 0)
                dialogueType = "Casual";
            else
                dialogueType = "Hint";
            ShowDialogueSetTimer(dialogueType, 0);
            dialogueState = "PreDialogue";
        }
    } // engage PreDialogue && Set le type de dialogue

    public void StartDialogue()
    {
        if ((PlayerController.pc.beingTalkedTo == null || PlayerController.pc.beingTalkedTo == gameObject) && ((dialogueState == "NoDialogue" && dialogueType == "Spawn") || dialogueState == "PreDialogue" || dialogueState == "InstantDialogue"))
        {
            ShowDialogueSetTimer(dialogueType, 1);
            dialogueState = "Dialogue";
        }
    } // engage Dialogue si entre dans la zone et que le pnj était encore en state "PreDialogue"

    void StartOutDialogue()
    {
        if ((PlayerController.pc.beingTalkedTo == null || PlayerController.pc.beingTalkedTo == gameObject) && dialogueState == "Dialogue")
        {
            ShowDialogueSetTimer(dialogueType, 2);
            dialogueState = "OutDialogue";
        }
    } // engage OutDialogue si on sort de la zone Dialogue alors qu'il était en train de parler


	//A NOTER : QUAND ON BARK PRES D'UN PNJ, CELUI-CI NOUS SORT UNE PHRASE DE DIALOGUE DE TYPE "DIALOGUE"


    
	public void SetMemento(Memento mementoParam)
	{
		memento = mementoParam;
		SetSpawnDialogue ();
		SetCasualDialogue ();
		SetHintDialogue ();
	}
	public void SetUIDialogueText(skDialogueUI text){
		UIDialogueText = text;
	}
	void SetSpawnDialogue(){
		switch (memento.ID) {
		case 0:
			spawnDialogues [0] = "spawn - 0 - 0";
			timeSpawnDialogues [0] = 3f;
			spawnDialogues [1] = "spawn - 0 - 1";
			timeSpawnDialogues [1] = 3f;
			spawnDialogues [2] = "spawn - 0 - 2";
			timeSpawnDialogues [2] = 3f;
			break;
		case 1:
			spawnDialogues [0] = "spawn - 1 - 0";
			timeSpawnDialogues [0] = 3f;
			spawnDialogues [1] = "Je suis Mickaël Mancini et je suis vraiment un super intervenant";
			timeSpawnDialogues [1] = 3f;
			spawnDialogues [2] = "spawn - 1 - 2";
			timeSpawnDialogues [2] = 3f;
			break;
		case 2:
			spawnDialogues [0] = "spawn - 2 - 0";
			timeSpawnDialogues [0] = 3f;
			spawnDialogues [1] = "spawn - 2 - 1";
			timeSpawnDialogues [1] = 3f;
			spawnDialogues [2] = "spawn - 2 - 2";
			timeSpawnDialogues [2] = 3f;
			break;
		case 3:
			spawnDialogues [0] = "spawn - 3 - 0";
			timeSpawnDialogues [0] = 3f;
			spawnDialogues [1] = "spawn - 3 - 1";
			timeSpawnDialogues [1] = 3f;
			spawnDialogues [2] = "spawn - 3 - 2";
			timeSpawnDialogues [2] = 3f;
			break;
		}
	}
	void SetCasualDialogue(){
		switch (memento.ID) {
		case 0:
			casualDialogues [0] = "casual - 0 - 0";
			timeCasualDialogues [0] = 3f;
			casualDialogues [1] = "casual - 0 - 1";
			timeCasualDialogues [1] = 3f;
			casualDialogues [2] = "casual - 0 - 2";
			timeCasualDialogues [2] = 3f;
			break;
		case 1:
			casualDialogues [0] = "casual - 1 - 0";
			timeCasualDialogues [0] = 3f;
			casualDialogues [1] = "casual - 1 - 1";
			timeCasualDialogues [1] = 3f;
			casualDialogues [2] = "casual - 1 - 2";
			timeCasualDialogues [2] = 3f;
			break;
		case 2:
			casualDialogues [0] = "casual - 2 - 0";
			timeCasualDialogues [0] = 3f;
			casualDialogues [1] = "casual - 2 - 1";
			timeCasualDialogues [1] = 3f;
			casualDialogues [2] = "casual - 2 - 2";
			timeCasualDialogues [2] = 3f;
			break;
		case 3:
			casualDialogues [0] = "casual - 3 - 0";
			timeCasualDialogues [0] = 3f;
			casualDialogues [1] = "casual - 3 - 1";
			timeCasualDialogues [1] = 3f;
			casualDialogues [2] = "casual - 3 - 2";
			timeCasualDialogues [2] = 3f;
			break;
		}
	}
	void SetHintDialogue(){
		switch (memento.ID) {
		case 0:
			hintDialogues [0] = "hint - 0 - 0";
			timeHintDialogues [0] = 3f;
			hintDialogues [1] = "hint - 0 - 1";
			timeHintDialogues [1] = 3f;
			hintDialogues [2] = "hint - 0 - 2";
			timeHintDialogues [2] = 3f;
			break;
		case 1:
			hintDialogues [0] = "hint - 1 - 0";
			timeHintDialogues [0] = 3f;
			hintDialogues [1] = "hint - 1 - 1";
			timeHintDialogues [1] = 3f;
			hintDialogues [2] = "hint - 1 - 2";
			timeHintDialogues [2] = 3f;
			break;
		case 2:
			hintDialogues [0] = "hint - 2 - 0";
			timeHintDialogues [0] = 3f;
			hintDialogues [1] = "hint - 2 - 1";
			timeHintDialogues [1] = 3f;
			hintDialogues [2] = "hint - 2 - 2";
			timeHintDialogues [2] = 3f;
			break;
		case 3:
			hintDialogues [0] = "hint - 3 - 0";
			timeHintDialogues [0] = 3f;
			hintDialogues [1] = "hint - 3 - 1";
			timeHintDialogues [1] = 3f;
			hintDialogues [2] = "hint - 3 - 2";
			timeHintDialogues [2] = 3f;
			break;
		}
	}
}