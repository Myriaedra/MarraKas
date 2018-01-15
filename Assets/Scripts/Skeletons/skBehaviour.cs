using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class skBehaviour : MonoBehaviour {
	NavMeshAgent navMesh;
	Rigidbody skRb;

	RubbleManager targetRubble;

	public Transform targetSpot;
	// Use this for initialization
	void Start () 
	{
		navMesh = GetComponent<NavMeshAgent> ();
		skRb = GetComponent<Rigidbody> ();
		targetRubble = FindObjectOfType<RubbleManager> ();
		targetSpot = targetRubble.GetSpotTransform ();
		MoveToRubble ();
	}
	
	// Update is called once per frame
	void Update () {
		if (navMesh.remainingDistance <= 0.1) 
		{
			navMesh.ResetPath ();
			transform.rotation = targetSpot.rotation;
		}
	}

	void MoveToRubble()
	{
		navMesh.SetDestination (targetSpot.position);
	}
		
}
