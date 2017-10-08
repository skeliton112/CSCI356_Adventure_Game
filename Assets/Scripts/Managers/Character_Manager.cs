using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager {

	Dictionary<string, int> states = new Dictionary<string, int> ();

	public int this[string s]{
		get {
			if (states.ContainsKey (s)) {
				return states [s];
			} else {
				return 0;
			}
		}
		set {
			states [s] = value;
		}
	}

	//Singleton pattern
	private static Character_Manager instance = null;
	public static Character_Manager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new Character_Manager();
			}
			return instance;
		}
	}
}
