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
		if (PlayerPrefs.HasKey("Name"))
			print(PlayerPrefs.GetString("Name"));
		else
			print("no way !");
	}

	// Use this for initialization
	public void StartNewGame()
	{
		SceneManager.LoadScene ("Beach_Quentin");
		PlayerPrefs.SetString ("Name", islandName.text);
	}

	public void BeginNameInput()
	{
		foreach (Button butt in buttons) 
		{
			butt.gameObject.SetActive (false);
		}
		islandName.gameObject.SetActive (true);
		islandName.ActivateInputField ();
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
