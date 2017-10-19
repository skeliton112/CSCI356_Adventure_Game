using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable {

	public int open_state;
	public string  target_scene;
	public Vector3 target_position;

	public override void Use (int item_id) {
		if (state == open_state && item_id < 0) {
			Game_Manager.Instance.ChangeScene (target_scene, target_position);
		} else {
			Conversation c = Dialogue_Manager.Instance [dialogue_data.name, state, item_id];
			Game_Manager.Instance.Start_Conversation (c, change_state);
		}
	}
}
