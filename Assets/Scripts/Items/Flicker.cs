using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {

	Light l;
	float time = 0;

	// Use this for initialization
	void Start () {
		l = GetComponent<Light> ();
		if (l == null)
			Destroy (this);
	}
	
	// Update is called once per frame
	void Update () {
		time += Game_Manager.deltaTime;
		l.intensity = 0.5f * (Mathf.Cos (time) + Mathf.Cos (5 * time) + Mathf.Cos (3.14159f * time)) + 10;
		l.transform.position = l.transform.parent.position + Vector3.up * (Mathf.Cos (time / 3) * 0.05f + 0.3f);
	}
}
