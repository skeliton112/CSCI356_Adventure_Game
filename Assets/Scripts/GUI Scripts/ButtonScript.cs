using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	public Text text;

	public void click () {
		Game_Manager.Instance.Load (text.text);
	}

	public void set_text (string t) {
		text.text = t;
	}
}
