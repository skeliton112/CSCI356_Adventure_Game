using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Printer : MonoBehaviour {

	public Text name_box, dialogue_box;

	public void Print (Line l) {
		name_box.text = "" + l.speaker;
		dialogue_box.text = l.text;
	}
}
