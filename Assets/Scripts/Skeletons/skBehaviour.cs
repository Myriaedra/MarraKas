using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class skBehaviour : MonoBehaviour {
	NavMeshAgent navMesh;
	Rigidbody skRb;
	public Memento memento;

	RubbleManager targetRubble;

	public Transform targetSpot;
	// Use this for initialization
	void Start () 
	{
		navMesh = GetComponent<NavMeshAgent> ();
		skRb = GetComponent<Rigidbody> ();
		targetRubble = FindObjectOfType<RubbleManager> ();
		targetSpot = targetRubble.GetSpotTransform ();
		if (targetSpot == null)
		{
			targetSpot = Camera.main.GetComponent<skSpotManager> ().GetSpotTransform ();
		}
		MoveToRubble ();
	}

	
	// Update is called once per frame
	void Update () {
		if (targetSpot != null && navMesh.remainingDistance <= 0.1) 
		{
			navMesh.ResetPath ();
			transform.rotation = targetSpot.rotation;
		}
	}

	void MoveToRubble()
	{
		if (targetSpot != null)
			navMesh.SetDestination (targetSpot.position);
	}

	public void SetMemento (Memento givenMemento)
	{
		memento = givenMemento;
	}
		
}
