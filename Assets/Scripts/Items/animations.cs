using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animations : MonoBehaviour {

	public GameObject leftArm, rightArm, body;
	public Character character;
	float timer;
	float duration;
	Interaction_Callback callback;
	string animation_name;

	void Start () {
		Game_Manager.Animation_Play += PlayAnimation;
	}

	// Update is called once per frame
	void Update () {
		timer += Game_Manager.deltaTime; 

		if (timer > duration && duration >= 0 && callback != null)
			callback (); 

		switch (animation_name) {

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

	public void PlayAnimation (Character c, string n){
		if (character == c) {
			duration = 0;
			animation_name = n;
			callback = null;
			timer = 0;
		} else {
			animation_name = "";
		}
	}

	public void PlayAnimation (string animationType, Interaction_Callback c, float d) {

		timer = 0;
		callback = c; 
		duration = d; 
		animation_name = animationType;

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


		Quaternion newRotation = Quaternion.AngleAxis (90, Vector3.right); 
		body.transform.rotation = Quaternion.Slerp(body.transform.rotation, newRotation, timer);

		//	body.transform.rotation = Quaternion.Euler (new Vector3 (90 * Mathf.Sin(timer * 20), 0, 0)); 

	}
}
