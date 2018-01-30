using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMessage : MonoBehaviour {
	public InputField[] lines;
	int currentLine;
	bool backspacing;
	public Text islandName;

	// Use this for initialization
	void Start () 
	{
		PlayerController.controlsAble = false;
		islandName.text = "From the island of " + PlayerPrefs.GetString ("Name");
		lines [0].ActivateInputField ();
		currentLine = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (lines [currentLine].text.Length >= lines [currentLine].characterLimit && currentLine < 4 && !backspacing) 
		{
			currentLine ++;
			lines [currentLine].ActivateInputField ();
		}

		if (lines [currentLine].text.Length <= 0 && Input.GetKeyDown (KeyCode.Backspace) && currentLine > 0) 
		{
			currentLine --;
			lines [currentLine].ActivateInputField ();
			char[] previousLine = lines [currentLine].text.ToCharArray();
			string lineContent = "";
			for (int i = 0; i < previousLine.Length - 1; i++) 
			{
				lineContent = lineContent + previousLine [i];
			}
			lines [currentLine].text = lineContent;
			lines [currentLine].Select ();
			StartCoroutine (MoveTextEnd_NextFrame ());
			backspacing = true;

		}

		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			if (currentLine < 4) {
				currentLine++;
				lines [currentLine].ActivateInputField ();
			} else {
				SendMessage ();
			}
		}
	}

	IEnumerator MoveTextEnd_NextFrame()
	{
		yield return 0; // Skip the first frame in which this is called.
		lines[currentLine].MoveTextEnd(false); // Do this during the next frame.
		backspacing = false;
	}

	void SendMessage()
	{
		//Store text value
		Destroy (gameObject);
		PlayerController.controlsAble = true;
	}
}
