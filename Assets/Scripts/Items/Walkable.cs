using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : Clickable {

	public override void MoveRequest (Vector3 position) {
		Player_Manager.Instance.set_path (position);
	}

	public override void UseRequest (Vector3 position) {}
	public override void UseXOnRequest (Vector3 position, Item item_id) {}
}
