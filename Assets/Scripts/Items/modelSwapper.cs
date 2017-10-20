using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelSwapper : MonoBehaviour {

	public GameObject[] masks;
	public GameObject pin;

	void Start () {
		Debug.Log (masks.Length);
		Inventory_Change i = new Inventory_Change ();
		i.item = Item.ox_mask;
		i.type = Action_Type.give;
		Player_Manager.Instance.item_change (i);
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; i++) {
			masks [i].SetActive (Player_Manager.Instance.current_mask == i);
			pin.SetActive (Player_Manager.Instance.items [5] != Item_state.free);
		}

		masks [4].SetActive (Player_Manager.Instance.current_mask != 3);
	}
}
