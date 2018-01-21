using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class skSpawner : MonoBehaviour {
	PartsReference pRef;
	// Use this for initialization
	void Start () {
		pRef = Camera.main.GetComponent<PartsReference> ();
		//SpawnFromParts (Random.Range(0,3), Random.Range(0,3), Random.Range(0,3));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnFromParts (int headID, int torsoID, int legID)
	{
		//Spawn 3 parts (with head and leg child of torso)
		GameObject spawnedTorso = Instantiate (pRef.torsoPrefabs [torsoID], new Vector3 (0, 0, 0), Quaternion.identity);
		GameObject spawnedHead = Instantiate(pRef.headPrefabs[headID], spawnedTorso.transform);
		GameObject spawnedLeg = Instantiate(pRef.legPrefabs[legID], spawnedTorso.transform);

		//Add the adder script to torso
		skPartAdder skPA = spawnedTorso.AddComponent<skPartAdder> ();
		//Add head and leg to torso
		skPA.AddLimb (spawnedHead, spawnedTorso);
		skPA.AddLimb (spawnedLeg, spawnedTorso);

		spawnedTorso.AddComponent<Rigidbody> ();
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
		skBehaviour skBh = torsoObj.AddComponent<skBehaviour> ();
		skBh.SetMemento (memento);
		torsoObj.GetComponentInChildren<Animator> ().enabled = true;
		skDialogueManager dialogueMan = torsoObj.AddComponent<skDialogueManager>();
		dialogueMan.mySkBehaviour = skBh;
		skBh.mySkDialogueManager = dialogueMan;
		dialogueMan.SetMemento (memento);
		dialogueMan.dialogueType = "Spawn";
        dialogueMan.StartDialogue();

		torsoObj.tag = "Skeleton";
	}
}
