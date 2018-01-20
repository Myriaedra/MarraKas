using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skDialogueManager : MonoBehaviour {

	public Memento memento;
	public string[] dialogues = new string[3];
	public skBehaviour mySkBehaviour;

	void Start(){
		
	}

	public void StartDialogue(int whichDialogue){
		mySkBehaviour.state = "Talking";
		print (dialogues [whichDialogue]);
	}

	public void SetMemento(Memento mementoParam)
	{
		memento = mementoParam;
		SetDialogue ();
	}
	void SetDialogue(){
		switch(memento.ID)
		{
		case 0:
			dialogues [0] = "0 - 0";
			dialogues [1] = "0 - 1";
			dialogues [2] = "0 - 2";
			break;
		case 1:
			dialogues [0] = "1 - 0";
			dialogues [1] = "1 - 1";
			dialogues [2] = "1 - 2";
			break;
		case 2:
			dialogues [0] = "2 - 0";
			dialogues [1] = "2 - 1";
			dialogues [2] = "2 - 2";
			break;
		case 3:
			dialogues [0] = "3 - 0";
			dialogues [1] = "3 - 1";
			dialogues [2] = "3 - 2";
			break;
		}
	}
}
