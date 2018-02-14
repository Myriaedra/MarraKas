using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Memento 
{

	public int ID;
	public string name;
	public int tempo;
	public float pitch;

	public Memento (int ID, string name, int tempo, float pitch)
	{
		this.ID = ID;
		this.name = name;
		this.tempo = tempo;
		this.pitch = pitch;
	}

}
