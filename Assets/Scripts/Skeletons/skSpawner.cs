using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class skSpawner : MonoBehaviour {
	PartsReference pRef;
	public skDialogueUI UIDialogueText;

	public DialogueSet[] dialogues = new DialogueSet[3];
    public Transform[] skSpotsBeforeRubble;

    bool[] skSpawned = new bool[3];
    skBehaviour[] readyToRubbleSetter = new skBehaviour[3];

	// Use this for initialization
	void Start () {
		pRef = Camera.main.GetComponent<PartsReference> ();
		//SpawnFromParts (Random.Range(0,3), Random.Range(0,3), Random.Range(0,3));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnFromParts (int headID, int torsoID, int legID, Memento memento, Vector3 position) //Used for false arrival
	{
		//Spawn 3 parts (with head and leg child of torso)
		GameObject spawnedTorso = Instantiate (pRef.torsoPrefabs [torsoID], position, Quaternion.identity);
		GameObject spawnedHead = Instantiate(pRef.headPrefabs[headID], spawnedTorso.transform);
		GameObject spawnedLeg = Instantiate(pRef.legPrefabs[legID], spawnedTorso.transform);

		//Add the adder script to torso
		skPartAdder skPA = spawnedTorso.AddComponent<skPartAdder> ();
		//Add head and leg to torso
		skPA.AddLimb (spawnedHead, spawnedTorso);
		skPA.AddLimb (spawnedLeg, spawnedTorso);

		CapsuleCollider capCo = spawnedTorso.AddComponent<CapsuleCollider> ();
		capCo.height = 2f;
		capCo.center = new Vector3 (0f, 1f, 0f);
		Rigidbody rB = spawnedTorso.AddComponent<Rigidbody> ();
		rB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		rB.isKinematic = true;
		//Behaviour
		spawnedTorso.AddComponent<NavMeshAgent>();
		spawnedTorso.AddComponent<AudioSource> ();
		skBehaviour skBh = spawnedTorso.AddComponent<skBehaviour> ();
		skBh.SetMemento (memento);

		spawnedTorso.GetComponentInChildren<Animator> ().enabled = true;

		/*skDialogueManager dialogueMan = spawnedTorso.AddComponent<skDialogueManager>();
		dialogueMan.mySkBehaviour = skBh;
		skBh.mySkDialogueManager = dialogueMan;
		dialogueMan.mySkSpawner = GetComponent<skSpawner>();
		dialogueMan.SetMemento (memento);
		dialogueMan.SetUIDialogueText (UIDialogueText);
		dialogueMan.dialogueType = "Casual";
		//dialogueMan.StartDialogue();*/

		spawnedTorso.tag = "Skeleton";
	}

	public void SpawnFromParts (GameObject headObj, GameObject torsoObj, GameObject legObj, Memento memento) 
	{
		
		//Add the adder script to torso
		skPartAdder skPA = torsoObj.AddComponent<skPartAdder> ();
		//Add head and leg to torso
		skPA.AddLimb (headObj, torsoObj);
		skPA.AddLimb (legObj, torsoObj);
		//Physics yeah !
		CapsuleCollider capCo = torsoObj.AddComponent<CapsuleCollider> ();
		capCo.height = 2f;
		capCo.center = new Vector3 (0f, 1f, 0f);
		Rigidbody rB = torsoObj.AddComponent<Rigidbody> ();
		rB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		rB.isKinematic = true;
		//Behaviour
		torsoObj.AddComponent<NavMeshAgent>();
		torsoObj.AddComponent<AudioSource> ();

		if (memento.ID == 0) 
		{
			skCaptainBehaviour skCBh  = torsoObj.AddComponent<skCaptainBehaviour> ();
			skCBh.SetMemento (memento);
			torsoObj.GetComponentInChildren<Animator> ().enabled = true;

			torsoObj.tag = "Captain";

		}
		else 
		{
			skBehaviour skBh = torsoObj.AddComponent<skBehaviour> ();
			skBh.SetMemento (memento);

			torsoObj.GetComponentInChildren<Animator> ().enabled = true;

			skDialogueManager dialogueMan = torsoObj.AddComponent<skDialogueManager>();
			dialogueMan.mySkBehaviour = skBh;
			skBh.mySkDialogueManager = dialogueMan;
            dialogueMan.mySkSpawner = GetComponent<skSpawner>();
            dialogueMan.SetMemento (memento);
            switch (memento.name)
            {
                case "Pedro":
                    skBh.mySpot = skSpotsBeforeRubble[0];
                    skSpawned[0] = true;
                    readyToRubbleSetter[0] = skBh;
                    break;
                case "Sancho":
                    skBh.mySpot = skSpotsBeforeRubble[1];
                    skSpawned[1] = true;
                    readyToRubbleSetter[1] = skBh;
                    break;
                case "Alessandro":
                    skBh.mySpot = skSpotsBeforeRubble[2];
                    skSpawned[2] = true;
                    readyToRubbleSetter[2] = skBh;
                    break;
            }
            dialogueMan.SetUIDialogueText (UIDialogueText);
			dialogueMan.dialogueType = "Spawn";
			dialogueMan.StartDialogue();

			torsoObj.tag = "Skeleton";


            if (skSpawned[0] && skSpawned[1] && skSpawned[2])
            {
                print("0");
                for (int i = 0; i < readyToRubbleSetter.Length; i++)
                {
                    readyToRubbleSetter[i].readyToRubble = true;
                    readyToRubbleSetter[i].MoveToRubble();
                }
            }
        }

	}
}
