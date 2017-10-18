using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallDrunk : MonoBehaviour {

	public GameObject leftArm, rightArm, body, mask;
	public Character character;
	float timer;
	float duration;

	Vector3 startPos; 
	Vector3 endPos; 
	float trajectoryHeight = 5; 
	// Use this for initialization
	void Start () {
		Vector3 startPos = mask.transform.position; 
		Vector3 endPos = mask.transform.position; 
		Debug.Log (startPos); 
	}
	
	// Update is called once per frame
	void Update () {
		timer += Game_Manager.deltaTime; 

		fallDrunkf ();
	}


	void fallDrunkf() {




		//Vector3 currentPos = Vector3.Lerp (startPos, endPos, timer); '
		Vector3 currentPos = mask.transform.position; 
		currentPos.y += trajectoryHeight * Mathf.Sin(Mathf.Clamp01(timer) * Mathf.PI);
		mask.transform.position = endPos;

	//	Quaternion newRotation = Quaternion.AngleAxis (90, Vector3.right); 
	//	body.transform.rotation = Quaternion.Slerp(body.transform.rotation, newRotation, timer);


	}
}
