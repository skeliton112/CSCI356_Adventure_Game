using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemScript : MonoBehaviour {

	ParticleSystem system;

	void Start () {
		system = GetComponent<ParticleSystem> ();
	}

	void Update () {
		if (Game_Manager.deltaTime == 0)
			system.Pause ();
		else
			system.Play ();
	}
}
