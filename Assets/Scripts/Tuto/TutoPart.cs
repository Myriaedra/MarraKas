using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoPart : MonoBehaviour {
	

	void OnDestroy()
	{
		transform.GetComponentInParent<TutoManager> ().CheckTutoPart (gameObject);
	}


}
