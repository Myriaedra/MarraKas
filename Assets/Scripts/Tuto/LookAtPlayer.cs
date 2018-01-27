using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (PlayerController.pc.transform.position - transform.position), 0.1f);
	}
}
