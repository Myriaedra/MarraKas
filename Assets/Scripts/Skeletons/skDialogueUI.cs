using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skDialogueUI : MonoBehaviour {

	Text myText;
    string dialogueName;

	AudioSource aS;
	public AudioClip[] xyloSFXs;


	void Start(){
		myText = GetComponent<Text> ();
		aS = GetComponent<AudioSource> ();
	}

	IEnumerator DisplayText(string sentence, int tempo, float pitch){
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

			if (letter != ' ' && Random.Range(0, tempo) >= 2) 
			{
				aS.pitch = pitch;
				aS.PlayOneShot(xyloSFXs[Random.Range(0, xyloSFXs.Length)]);
			}
			yield return new WaitForSeconds (0.02f);
		}
	}

	public void ClearDisplay(){
		myText.text = "";
	}

	public void StartDisplaying(string newSentence, Memento memento){
		StopAllCoroutines ();
		StartCoroutine (DisplayText (newSentence, memento.tempo, memento.pitch));
	}
}
