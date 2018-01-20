using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skDialogueManager : MonoBehaviour {

	public Memento memento;

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
	string dialogueState; //(NoDialogue, PreDialogue, Dialogue, OutDialogue)
	public string dialogueType; //(Spawn, Casual, Hint)



	void Start(){
		StartCoroutine (CheckDistanceToPlayer ());
	}

	void Update(){
		switch(dialogueState){
		case "PreDialogue":
			timer -= Time.deltaTime;
			if(timer<=0){
				dialogueState = "NoDialogue";
				mySkBehaviour.state = "Moving";
			}
			break;
		}
	}

	public void StartDialogue(string typeDialogue /*(Spawn, Casual, Hint)*/, int whichDialogue){
		PlayerController.pc.beingTalkedTo = true;
		mySkBehaviour.state = "Talking";
		switch(typeDialogue){
		case "Spawn":
			print (spawnDialogues [whichDialogue]);
			break;
		case "Casual":
			print (casualDialogues [whichDialogue]);
			break;
		case "Hint":
			print (hintDialogues [whichDialogue]);
			break;
		}
	}

	IEnumerator CheckDistanceToPlayer(){
		while(true){
			yield return new WaitForSeconds (0.5f);
			actualDistance = Vector3.Distance (transform.position, PlayerController.pc.transform.position);

			//QUAND ON EST LOIN DU PNJ------------------------------------------------------------------------------------------
			if(actualDistance>=distancePreDialogue && playerZone!="NoDialogue"){
				playerZone = "NoDialogue";
				print (playerZone);
			}

			//QUAND ON RENTRE DANS LA ZONE POUR SE FAIRE INTERPELLER------------------------------------------------------------
			else if(actualDistance<=distancePreDialogue && actualDistance>distanceDialogue && playerZone!="PreDialogue"){
				//ET QU'ON ETAIT LOIN DU PNJ --> DIALOGUE "INTERPELLER" EH TOI ! :D "
				if(playerZone=="NoDialogue"){
					if (!PlayerController.pc.beingTalkedTo) {
						if (Random.Range (0, 2) == 0)
							dialogueType = "Casual";
						else
							dialogueType = "Hint";
						StartDialogue (dialogueType, 0);
						dialogueState = "PreDialogue";
						timer = 4f;
					}					
				}
				//ET QU'ON ETAIT PROCHE DU PNJ --> DIALOGUE "AH... SALUT"
				else if(playerZone=="Dialogue"){
					if (!PlayerController.pc.beingTalkedTo) {
						StartDialogue (dialogueType, 2);
						dialogueState = "OutDialogue";
					}
				}
				playerZone = "PreDialogue";
				print (playerZone);
			}

			//QUAND ON EST PROCHE DU PNJ---------------------------------------------------------------------------------------
			else if(actualDistance<=distanceDialogue && playerZone!="Dialogue"){
				if (!PlayerController.pc.beingTalkedTo) {
					StartDialogue (dialogueType, 1);
					dialogueState = "Dialogue";
				}
				playerZone = "Dialogue";
				print (playerZone);
			}
		}
	}

	//A NOTER : QUAND ON BARK PRES D'UN PNJ, CELUI-CI NOUS SORT UNE PHRASE DE DIALOGUE DE TYPE "DIALOGUE"

	public void SetMemento(Memento mementoParam)
	{
		memento = mementoParam;
		SetSpawnDialogue ();
		SetCasualDialogue ();
		SetHintDialogue ();
	}
	void SetSpawnDialogue(){
		switch (memento.ID) {
		case 0:
			spawnDialogues [0] = "spawn - 0 - 0";
			timeSpawnDialogues [0] = 1f;
			spawnDialogues [1] = "spawn - 0 - 1";
			timeSpawnDialogues [1] = 1f;
			spawnDialogues [2] = "spawn - 0 - 2";
			timeSpawnDialogues [2] = 1f;
			break;
		case 1:
			spawnDialogues [0] = "spawn - 1 - 0";
			timeSpawnDialogues [0] = 1f;
			spawnDialogues [1] = "spawn - 1 - 1";
			timeSpawnDialogues [1] = 1f;
			spawnDialogues [2] = "spawn - 1 - 2";
			timeSpawnDialogues [2] = 1f;
			break;
		case 2:
			spawnDialogues [0] = "spawn - 2 - 0";
			timeSpawnDialogues [0] = 1f;
			spawnDialogues [1] = "spawn - 2 - 1";
			timeSpawnDialogues [1] = 1f;
			spawnDialogues [2] = "spawn - 2 - 2";
			timeSpawnDialogues [2] = 1f;
			break;
		case 3:
			spawnDialogues [0] = "spawn - 3 - 0";
			timeSpawnDialogues [0] = 1f;
			spawnDialogues [1] = "spawn - 3 - 1";
			timeSpawnDialogues [1] = 1f;
			spawnDialogues [2] = "spawn - 3 - 2";
			timeSpawnDialogues [2] = 1f;
			break;
		}
	}
	void SetCasualDialogue(){
		switch (memento.ID) {
		case 0:
			casualDialogues [0] = "casual - 0 - 0";
			timeCasualDialogues [0] = 1f;
			casualDialogues [1] = "casual - 0 - 1";
			timeCasualDialogues [1] = 1f;
			casualDialogues [2] = "casual - 0 - 2";
			timeCasualDialogues [2] = 1f;
			break;
		case 1:
			casualDialogues [0] = "casual - 1 - 0";
			timeCasualDialogues [0] = 1f;
			casualDialogues [1] = "casual - 1 - 1";
			timeCasualDialogues [1] = 1f;
			casualDialogues [2] = "casual - 1 - 2";
			timeCasualDialogues [2] = 1f;
			break;
		case 2:
			casualDialogues [0] = "casual - 2 - 0";
			timeCasualDialogues [0] = 1f;
			casualDialogues [1] = "casual - 2 - 1";
			timeCasualDialogues [1] = 1f;
			casualDialogues [2] = "casual - 2 - 2";
			timeCasualDialogues [2] = 1f;
			break;
		case 3:
			casualDialogues [0] = "casual - 3 - 0";
			timeCasualDialogues [0] = 1f;
			casualDialogues [1] = "casual - 3 - 1";
			timeCasualDialogues [1] = 1f;
			casualDialogues [2] = "casual - 3 - 2";
			timeCasualDialogues [2] = 1f;
			break;
		}
	}
	void SetHintDialogue(){
		switch (memento.ID) {
		case 0:
			hintDialogues [0] = "hint - 0 - 0";
			timeHintDialogues [0] = 1f;
			hintDialogues [1] = "hint - 0 - 1";
			timeHintDialogues [1] = 1f;
			hintDialogues [2] = "hint - 0 - 2";
			timeHintDialogues [2] = 1f;
			break;
		case 1:
			hintDialogues [0] = "hint - 1 - 0";
			timeHintDialogues [0] = 1f;
			hintDialogues [1] = "hint - 1 - 1";
			timeHintDialogues [1] = 1f;
			hintDialogues [2] = "hint - 1 - 2";
			timeHintDialogues [2] = 1f;
			break;
		case 2:
			hintDialogues [0] = "hint - 2 - 0";
			timeHintDialogues [0] = 1f;
			hintDialogues [1] = "hint - 2 - 1";
			timeHintDialogues [1] = 1f;
			hintDialogues [2] = "hint - 2 - 2";
			timeHintDialogues [2] = 1f;
			break;
		case 3:
			hintDialogues [0] = "hint - 3 - 0";
			timeHintDialogues [0] = 1f;
			hintDialogues [1] = "hint - 3 - 1";
			timeHintDialogues [1] = 1f;
			hintDialogues [2] = "hint - 3 - 2";
			timeHintDialogues [2] = 1f;
			break;
		}
	}
}