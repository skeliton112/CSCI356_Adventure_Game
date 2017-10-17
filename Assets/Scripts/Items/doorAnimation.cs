using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimation : MonoBehaviour {


	public GameObject leftDoor, rightDoor, bar; 
	float timer; 
	float duration; 
	Interaction_Callback callback; 
	public Renderer barRend; // play game

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Game_Manager.deltaTime; 
		doorOpen (); 
	}

	void doorOpen() {

		barRend.enabled = false; 


		Quaternion newRotationL = Quaternion.AngleAxis (-70, Vector3.down); 
		leftDoor.transform.rotation = Quaternion.Slerp(leftDoor.transform.rotation, newRotationL, timer/50); 

		Quaternion newRotationR = Quaternion.AngleAxis (70, Vector3.down); 
		rightDoor.transform.rotation = Quaternion.Slerp(rightDoor.transform.rotation, newRotationR, timer/50); 

	
		
	}
}
