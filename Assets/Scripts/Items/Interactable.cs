using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void state_change (int state);

public class Interactable : Clickable {
	
	public TextAsset dialogue_data;
	public Use_Type use_type;
	public string object_name;

	public event state_change state_change_event;

	int state = 0;

	public Vector3 position {
		get {
			return transform.position;
		}
	}

	void Start () {
		Dialogue_Manager.register (dialogue_data.name);
	}

	string verb () {
		if (Game_Manager.Instance.has_selection) {
			switch (use_type) {
			case Use_Type.talkto:
				return "Give " + Game_Manager.Instance.selected_item + " to ";
			default:
				return "Use " + Game_Manager.Instance.selected_item + " with ";
			}
		}else{
			switch (use_type) {
			case Use_Type.gothrough:
				return "Go through ";
			case Use_Type.inspect:
				return "Inspect ";
			case Use_Type.pickup:
				return "Pick up ";
			case Use_Type.talkto:
				return "Talk to ";
			case Use_Type.use:
			default:
				return "Use ";
			}
		}
	}

	void OnMouseEnter()
	{
		GUI_Manager.Instance.UpdateText(verb () + object_name);
	}

	void OnMouseExit()
	{
		GUI_Manager.Instance.UpdateText("");
	}

	public void change_state (int s){
		state = s;
		if (state_change_event != null)
			state_change_event (state);
	}

	public void Use (int item_id) {
		Conversation c = Dialogue_Manager.Instance [dialogue_data.name, state, item_id];
		Game_Manager.Instance.Start_Conversation (c, change_state);
	}

	public override void MoveRequest (Vector3 position) {
		Player_Manager.Instance.set_path (position);
	}

	public override void UseRequest (Vector3 position) {
		Player_Manager.Instance.set_path (position);
		Player_Manager.Instance.set_target (this);
	}

	public override void UseXOnRequest (Vector3 position, Item item_id) {
		Player_Manager.Instance.set_path (position);
		Player_Manager.Instance.set_target (this, (int)item_id);
	}
}
