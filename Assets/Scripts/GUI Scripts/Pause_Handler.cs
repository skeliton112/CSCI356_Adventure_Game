using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Handler : MonoBehaviour {

	public GameObject pauseMenu, normalGUI1, normalGUI2;

	// Update is called once per frame
	void Update () {
		if (Game_Manager.Instance.menu_up) {
			pauseMenu.SetActive (true);
			normalGUI1.SetActive (false);
			normalGUI2.SetActive (false);
		} else {
			pauseMenu.SetActive (false);
			normalGUI1.SetActive (true);
			normalGUI2.SetActive (true);
		}
	}
}
