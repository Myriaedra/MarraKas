using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	Button[] buttons;
	InputField islandName;

	void Start()
	{
		buttons = GetComponentsInChildren<Button> ();
		islandName = GetComponentInChildren<InputField> ();
		islandName.gameObject.SetActive (false);
	}

	// Use this for initialization
	public void StartNewGame()
	{
		SceneManager.LoadScene ("Beach_rubble");
	}

	public void BeginNameInput()
	{
		foreach (Button butt in buttons) 
		{
			butt.gameObject.SetActive (false);
		}
		islandName.gameObject.SetActive (true);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
