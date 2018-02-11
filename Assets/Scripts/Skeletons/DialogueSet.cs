using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSet{

	public string[] spawnDialogues = new string[3];
	public float[] timeSpawnDialogues = new float[3];

	public string[] casualDialogues = new string[3];
	public float[] timeCasualDialogues = new float[3];

	public string[] hintDialogues = new string[3];
	public float[] timeHintDialogues = new float[3];

    public string[] helpDialogues = new string[3];
    public float[] timeHelpDialogues = new float[3];

}
