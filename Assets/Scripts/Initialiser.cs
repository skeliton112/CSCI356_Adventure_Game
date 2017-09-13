using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialiser : MonoBehaviour {

	public Walker player;

	// Use this for initialization
	void Start () {
		Player_Manager.Instance.set_player (player);
		Destroy (this.gameObject);
	}
}
