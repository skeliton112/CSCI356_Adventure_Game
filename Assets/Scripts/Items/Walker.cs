using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {
	
	protected List<Vector3> waypoints = new List<Vector3>();
	public float speed;
	public Vector3 position {
		get {
			return transform.position;
		}
	}
	Interaction_Callback callback = null;

	public virtual void set_path (List<Vector3> path, Interaction_Callback c = null) {
		waypoints = path;
		callback = c;
	}

	void Update (){
		Move ();
	}

	protected void Move () {
		if (waypoints.Count > 0) {
			Vector3 direction = waypoints [0] - transform.position;
			if (direction.sqrMagnitude > speed * Game_Manager.deltaTime * speed * Game_Manager.deltaTime) {
				direction.Normalize ();
				direction *= speed * Game_Manager.deltaTime;
			} else {
				waypoints.RemoveAt (0);
			}
			transform.Translate (direction);
		} else if (callback != null) {
			callback ();
			callback = null;
		}
	}
}
