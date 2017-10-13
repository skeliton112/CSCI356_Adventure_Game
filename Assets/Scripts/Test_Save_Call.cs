using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Save_Call : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update(){
		//TESTING
		if(Input.GetKeyDown("s"))
		{
			Game_Manager.Instance.Save();
		}

		if(Input.GetKeyDown("l"))
		{
			Game_Manager.Instance.Load();
		}

		if(Input.GetKeyDown("p"))
		{
			Game_Manager.Instance.PrintStates();
		}

		if(Input.GetKeyDown("r"))
		{
			Game_Manager.Instance.reset();
		}
	}
}
