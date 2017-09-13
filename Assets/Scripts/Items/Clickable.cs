using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clickable : MonoBehaviour {

	public abstract void MoveRequest (Vector3 position);
	public abstract void UseRequest (Vector3 position);
	public abstract void UseXOnRequest (Vector3 position, Item item_id);

	void OnMouseOver () {
		RaycastHit hit;
		Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100);

		if (Game_Manager.Instance.WantsToMove ()) {
			MoveRequest (hit.point);
		}
		if (Game_Manager.Instance.WantsToUse ()) {
			UseRequest (hit.point);
		}
		if (Game_Manager.Instance.WantsToUseXOn ()) {
			UseXOnRequest (hit.point, Game_Manager.Instance.selected_item);
		}
	}	
}
