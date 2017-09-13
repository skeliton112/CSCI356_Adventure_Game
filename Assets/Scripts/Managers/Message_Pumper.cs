using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message_Pumper : MonoBehaviour {

	void Start () {

	}

	void Update () {
		Game_Manager.Instance.Update ();
		GUI_Manager.Instance.Update();
	}

	void OnGUI () {
		GUI_Manager.Instance.OnGUI ();
	}
}
