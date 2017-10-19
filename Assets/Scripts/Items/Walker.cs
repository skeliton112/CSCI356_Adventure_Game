using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {
	
	protected List<Vector3> waypoints = new List<Vector3>();
	public float speed;
	public bool is_paused;
	public Transform rotator;
	public Vector3 position {
		get {
			return transform.position;
		}
	}
	public Vector3 direction {
		get {
			if (waypoints.Count == 0) {
				return Vector3.zero;
			}else{
				return Vector3.Normalize(position - waypoints[0]);
			}
		}
	}
	private Interaction_Callback callback = null;

	public virtual void set_path (List<Vector3> path, Interaction_Callback c = null) {
		waypoints = path;
		callback = c;
	}

	public void clear_path () {
		waypoints.Clear ();
	}

	void Start()
	{
		is_paused = false;
	}

	void Update (){
		Move ();
	}

	protected void Move () {
		if(is_paused)
			return;
		if (waypoints != null && waypoints.Count > 0) {
			Vector3 direction = waypoints [0] - transform.position;
			rotator.LookAt(waypoints[0]);
			if (direction.sqrMagnitude > speed * Game_Manager.deltaTime * speed * Game_Manager.deltaTime) {
				direction.Normalize ();
				direction *= speed * Game_Manager.deltaTime;
			} else {
				waypoints.RemoveAt (0);
			}
			transform.Translate (direction, Space.World);
		} else if (callback != null) {
			Interaction_Callback temp = new Interaction_Callback (callback);
			callback ();
			if (temp == callback)
				callback = null;
		}
	}
}
