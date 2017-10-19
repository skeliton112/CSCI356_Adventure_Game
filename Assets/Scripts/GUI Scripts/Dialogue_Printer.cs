using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Printer : MonoBehaviour {

	public Text name_box, dialogue_box;

	string text = "";
	float timer = 0;

	void Update () {
		timer += Game_Manager.deltaTime;
		dialogue_box.text = process (text);
	}

	string process (string s){
		if (GUI_Manager.Instance.finished_playing == true)
			return s.Replace ("|", "");
		int count = 0;
		int i;
		for (i = 0; i < s.Length; i++) {
			if (s [i] == '|')
				count++;
			if (count > 3 * timer)
				break;
		}
		if (i == s.Length)
			GUI_Manager.Instance.finished_playing = true;
		string temp = s.Substring (0, i);
		return temp.Replace ("|", "");
	}

	public void Print (Line l) {
		name_box.text = "" + l.speaker;
		text = l.text;
		timer = 0;
	}
}
