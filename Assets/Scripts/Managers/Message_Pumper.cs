using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message_Pumper : MonoBehaviour {

	public GameObject player;

	void Start () {
		Player_Manager.Instance.set_player (player);
	}

	void Update () {
		Game_Manager.Instance.Update ();
		GUI_Manager.Instance.Update();
	}

	void OnGUI () {
		GUI_Manager.Instance.OnGUI ();
	}
}
