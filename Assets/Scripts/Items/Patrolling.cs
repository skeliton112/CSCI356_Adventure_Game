using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum action_type { walk_to, perform_animation }

[System.Serializable()]
public struct action
{
	action_type type;
	Vector3 walk_target;
	string animation_name;
};

public class Patrolling : MonoBehaviour {
	public float min_distance;
	public Transform[] waypoints;

	private int currentPoint = 0;
	private Vector3 target;
	private Vector3 direction;
	private Walker character;

	void Start () {
		character = gameObject.GetComponent <Walker> ();
		target = waypoints [currentPoint].position;
		patrol_path ();
	}


	void Update () {
		direction = Vector3.Normalize(target - transform.position);
		Vector3 distance = Player_Manager.Instance.position - transform.position;

		float dot_product = Vector3.Dot(direction,distance);

		if(distance.magnitude < min_distance && dot_product > Mathf.Abs(Vector3.Dot (Vector3.Cross(Vector3.up, direction), distance)))
		{
			character.is_paused = true;
		}
		else{
			character.is_paused = false;
		}

		dot_product = Vector3.Dot(Player_Manager.Instance.direction,distance);
		if(distance.magnitude < min_distance && dot_product > Mathf.Abs(Vector3.Dot (Vector3.Cross(Vector3.up, Player_Manager.Instance.direction), distance)))
		{
			Player_Manager.Instance.clear_path();
		}
	}

	void change_waypoint () {
		currentPoint++;

		if (currentPoint >= waypoints.Length)
			currentPoint = 0;
		target = waypoints [currentPoint].position;
		patrol_path();
	}

	void patrol_path()
	{
		character.set_path(WalkSystem.Instance.get_path (gameObject.transform.position, target), change_waypoint);
	}
}
