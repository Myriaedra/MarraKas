using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skDialogueUI : MonoBehaviour {

	Text myText;
    string dialogueName;


	void Start(){
		myText = GetComponent<Text> ();
	}

	IEnumerator DisplayText(string sentence){
        myText.text = "";
        int nbChar = 0;
		foreach(char letter in sentence.ToCharArray ()){
			myText.text += letter;
			nbChar++;
			if(nbChar>38 && letter == ' '){
				nbChar = 0;
				yield return new WaitForSeconds (0.75f);
				myText.text = "" + letter;				
			}
			yield return new WaitForSeconds (0.02f);
		}
	}

	public void ClearDisplay(){
		myText.text = "";
	}

	public void StartDisplaying(string newSentence, string newName){
		StopAllCoroutines ();
        dialogueName = newName;
        StartCoroutine (DisplayText (newSentence));
	}
}
