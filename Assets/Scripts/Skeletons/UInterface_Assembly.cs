using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UInterface_Assembly : MonoBehaviour {
	public Color offColor;
	public Color onColor;

	public Image[] arrows1;
	public Image[] arrows2;
	public Text mementoName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void UpdateArrows(int newPosition, int previousPosition) {
		arrows1 [newPosition].color = onColor;
		arrows2 [newPosition].color = onColor;

		arrows1 [previousPosition].color = offColor;
		arrows2 [previousPosition].color = offColor;

	}

	public void UpdateName (Memento memento)
	{
		mementoName.text = memento.name;
	}

	public void InitArrows(Memento memento)
	{
		for (int i = 0; i < 4; i++) 
		{
			if (i == 0) {
				arrows1 [i].color = onColor;
				arrows2 [i].color = onColor;
			} 
			else 
			{
				arrows1 [i].color = offColor;
				arrows2 [i].color = offColor;
			}
		}
		mementoName.text = memento.name;
			
	}
}
