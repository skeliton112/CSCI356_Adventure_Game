using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour {
	public Transform[] waypoints;
	private int currentPoint = 0;
	private Vector3 target;
	private Vector3 direction;

	Walker character;
	void Start () {
		character = gameObject.GetComponent <Walker> ();
		target = waypoints [currentPoint].position;
		patrol_path ();
	}
	

	void Update () {
		direction = target - transform.position;
	}

	void change_waypoint () {
		currentPoint++;
				
		if (currentPoint >= waypoints.Length)
			currentPoint = 0;
		target = waypoints [currentPoint].position;
		Debug.Log (target);
		Debug.Log (currentPoint);
		patrol_path();
	}

	void patrol_path()
	{
		character.set_path(WalkSystem.Instance.get_path (gameObject.transform.position, target), change_waypoint);
	}
}
