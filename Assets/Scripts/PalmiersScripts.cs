using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmiersScripts : MonoBehaviour {

	public Rigidbody[] rbNoixDeCoco;

	public void NoixDeCocoFall(){
		for (int i = 0; i < rbNoixDeCoco.Length; i++) {
			rbNoixDeCoco [i].isKinematic = false;
		}
	}
}
