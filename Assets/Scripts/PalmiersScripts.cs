using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmiersScripts : MonoBehaviour {

	public List<Rigidbody> rbNoixDeCoco = new List<Rigidbody>();

	public void NoixDeCocoFall(){
		for (int i = 0; i < rbNoixDeCoco.Count; i++) {
			rbNoixDeCoco [i].isKinematic = false;
		}
	}

	public void OneNoixDeCocoFall(){
		if(rbNoixDeCoco.Count>0){
			rbNoixDeCoco [0].isKinematic = false;
			rbNoixDeCoco.RemoveAt (0);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			OneNoixDeCocoFall ();
		}
	}
}
