using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

		Quaternion myTargetrotation;

		void Start () 
		{

		}

		// Update is called once per frame
		void FixedUpdate () 
		{
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y+0.5f, transform.rotation.eulerAngles.z);
		}
}
