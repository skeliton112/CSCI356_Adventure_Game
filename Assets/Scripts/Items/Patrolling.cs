using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum action_type { walk_to, play_animation, look_at }

[System.Serializable()]
public struct action
{
	public action_type type;
	public Vector3 target;
	public string animation_name;
	public float animation_duration;
};

public class Patrolling : MonoBehaviour {
	public float min_distance;
	public action[] waypoints;
	private int currentPoint = 0;
	private Vector3 target;
	private Vector3 direction;

	Walker character;
	animations animator;

	void Start () {
		character = gameObject.GetComponent <Walker> ();
		animator = gameObject.GetComponent <animations> ();
		next_action ();
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

	void next_action () {
		currentPoint++;
		if (currentPoint >= waypoints.Length)
			currentPoint = 0;

		switch (waypoints[currentPoint].type){
		case action_type.walk_to:
			target = waypoints [currentPoint].target;
			patrol_path ();
			break;
		case action_type.play_animation:
			animator.PlayAnimation (waypoints [currentPoint].animation_name, next_action, waypoints [currentPoint].animation_duration);
			break;
		case action_type.look_at:
			break;
		}
	}

	void patrol_path()
	{
		character.set_path(WalkSystem.Instance.get_path (gameObject.transform.position, target), next_action);
	}
}
