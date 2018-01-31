using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowJump : MonoBehaviour {

	public SkinnedMeshRenderer skinnedMeshRend;
	public SpriteRenderer mySpriteRend;
	float myOpacity = 0;
	float actualOpacity = 0;

	public float maxOpacity;
	public float lerpValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//ActualizeOpacityGroundShadow();
		//ActualizePositionGroundShadow ();
		//ActualizeShadowCasting ();
	}

	void ActualizeOpacityGroundShadow(){
		actualOpacity = myOpacity;
		if(PlayerController.pc.landed && myOpacity>0.0f){
			myOpacity = Mathf.Lerp (myOpacity, 0.0f, lerpValue);
		}
		else if(!PlayerController.pc.landed && myOpacity<maxOpacity){
			myOpacity = Mathf.Lerp (myOpacity, maxOpacity, lerpValue);
		}
		if(actualOpacity!=myOpacity){
			Color c = mySpriteRend.color;
			mySpriteRend.color = new Color (c.r, c.g, c.b, myOpacity);
		}
	}

	void ActualizePositionGroundShadow(){
		if(!PlayerController.pc.landed){
			RaycastHit hit;
			int layerMask = 1 << 8;
			layerMask = ~layerMask;
			if(Physics.Raycast (PlayerController.pc.transform.position + PlayerController.pc.transform.forward*0.4f, -Vector3.up, out hit, 50f, layerMask)){
				transform.position = hit.point + new Vector3(0, 0.02f, 0);
			}
		}
	}

	void ActualizeShadowCasting(){
		if (PlayerController.pc.landed){
			print ("yop");
			skinnedMeshRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}
		else{
			print ("nope");
			skinnedMeshRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;	
		}
	}
}
