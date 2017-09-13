using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnStateEnter : MonoBehaviour {

	public int trigger_state;

	void Start () {
		Interactable i = GetComponent<Interactable> ();
		if (i != null) {
			i.state_change_event += OnStateChange;
		}
	}

	void OnStateChange (int state) {
		if (state == trigger_state)
			Destroy (this.gameObject);
	}
}
