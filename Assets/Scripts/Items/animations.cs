using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animations : MonoBehaviour {

	public GameObject leftArm, rightArm, body; 
	float timer;
	float duration;
	Interaction_Callback callback;
	
	// Update is called once per frame
	void Update () {
		timer += Game_Manager.deltaTime; 

		if (timer > duration && callback != null)
			callback (); 

		fallDrunk (); 
	}

	public void PlayAnimation (string animationType, Interaction_Callback c, float d) {

		timer = 0; 
		callback = c; 
		duration = d; 

		switch (animationType) {

		case "wave":
			wave (); 
			break; 

		case "angry":
			angry (); 
			break; 

		case "hammering":
			hammering (); 
			break;

		case "happy":
			happy (); 
			break;

		case "fallDrunk":
			fallDrunk (); 
			break; 

		}

	}

	void wave() {

		leftArm.transform.rotation = Quaternion.Euler(new Vector3(0,0,30 * Mathf.Sin(timer * 20))); 
		rightArm.transform.rotation = Quaternion.Euler(new Vector3(0,0,-30 * Mathf.Sin(timer * 20))); 
	}

	void angry() {
		leftArm.transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0)); 
		leftArm.transform.rotation *= Quaternion.Euler(new Vector3(0, 0 , 30 * Mathf.Sin(timer * 20))); 
		rightArm.transform.rotation = Quaternion.Euler (new Vector3 (0, -40, -20));

	}

	void hammering() {
		leftArm.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0)); 
		leftArm.transform.rotation *= Quaternion.Euler(new Vector3(0, 0 , 45 * Mathf.Sin(timer * 20)));

	}

	void happy() {
		leftArm.transform.rotation = Quaternion.Euler(new Vector3(0,0,35 * Mathf.Sin(timer * 20))); 
		rightArm.transform.rotation = Quaternion.Euler(new Vector3(0,0,35 * Mathf.Sin(timer * 20))); 

	}
	void fallDrunk() {

		body.transform.rotation = Quaternion.Euler (new Vector3 (90 * Mathf.Sin(timer * 20), 0, 0)); 

	}
}
