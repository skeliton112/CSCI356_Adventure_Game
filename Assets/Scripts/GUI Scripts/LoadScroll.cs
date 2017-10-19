using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadScroll : MonoBehaviour {

	public GameObject button;

	public void Refresh () {
		string[] files;
		files = Directory.GetFiles ("Assets/Saves");

		for (int i = 0; i < files.Length; i++) {
			if (files [i].EndsWith ("meta"))
				continue;
			Debug.Log (files [i]);
			GameObject temp = Instantiate (button, transform);
			string name = files [i].Substring (files [i].LastIndexOf ("\\") + 1);
			name = name.Substring (0, name.Length - 4);
			temp.SendMessage ("set_text", name);
		}
	}
}
