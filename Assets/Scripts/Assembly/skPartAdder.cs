using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skPartAdder : MonoBehaviour {

	public void AddLimb( GameObject BonedObj, GameObject RootObj)
	{
		SkinnedMeshRenderer[] BonedObjects = BonedObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer SkinnedRenderer in BonedObjects)
			ProcessBonedObject( SkinnedRenderer, RootObj );
		Destroy (BonedObj);
	}

	void ProcessBonedObject (SkinnedMeshRenderer ThisRenderer, GameObject RootObj)
	{
		{
			/*      Create the SubObject        */
			GameObject NewObj = new GameObject( transform.gameObject.name );
			NewObj.transform.parent = RootObj.transform;
			/*      Add the renderer        */
			NewObj.AddComponent<SkinnedMeshRenderer>();
			SkinnedMeshRenderer NewRenderer = NewObj.GetComponent<SkinnedMeshRenderer>();
			/*      Assemble Bone Structure     */
			Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
			for ( int i=0; i< ThisRenderer.bones.Length ; i++ )
				MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, RootObj.transform );
			/*      Assemble Renderer       */
			NewRenderer.bones = MyBones;
			NewRenderer.sharedMesh = ThisRenderer.sharedMesh;
			NewRenderer.materials = ThisRenderer.materials;
		}
	}

	Transform FindChildByName( string ThisName, Transform ThisGObj)
	{
		Transform ReturnObj;
		if( ThisGObj.name==ThisName )
			return ThisGObj.transform;
		foreach (Transform child in ThisGObj)
		{
			ReturnObj = FindChildByName( ThisName, child );
			if( ReturnObj )
				return ReturnObj;
		}
		return null;
	}

}
