using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperSphereScript : MonoBehaviour {

	public Transform target;
	public GameObject confettiPart;
	BoxCollider myCollider;
	[HideInInspector]
	public bool bumpEnabled = false;
	Rigidbody myRb;
	public ParticleSystem partUp;


	void Start(){
		myCollider = GetComponent<BoxCollider> ();
		myRb = GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter(Collider other){
		if(other.name == "BumpTriggerChecker" && !bumpEnabled){
			StartCoroutine (LerpTowardHoleCenter ());
		}
		if(other.tag == "Player" && bumpEnabled && Vector3.Distance (transform.position, PlayerController.pc.transform.position)<3f){
			PlayerController.pc.GetComponent<Rigidbody> ().velocity += new Vector3(0, 17, 0);
		}
	}

	IEnumerator LerpTowardHoleCenter(){
		myCollider.enabled = true;
		myRb.isKinematic = true;
		GameObject confettiPartInstance = Instantiate (confettiPart, target.position, Quaternion.Euler (-90, 0, 0));
		Destroy (confettiPartInstance, 2.5f);
		partUp.Play ();
		for (int i = 1; i < 10; i++) {
			transform.position = Vector3.Lerp (transform.position, target.position, 0.3f);
			yield return new WaitForSeconds (0.05f);
		}
		bumpEnabled = true;
	}
}
