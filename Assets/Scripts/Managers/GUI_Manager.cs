﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Manager {
	
	Inventory_Item[] item_GUI = new Inventory_Item[12];

	GameObject inventory, dialogue;

	private GUI_Manager () {
		inventory = GameObject.FindGameObjectWithTag ("inventory");
		dialogue = GameObject.FindGameObjectWithTag ("dialogue_box");
	}

	public void Update()
	{
		//if(Input.GetKeyDown("i"))
		//	ChangeGuiInventoryVisibility();

		if (Game_Manager.Instance.State == Game_state.walking) {
			inventory.transform.localScale = new Vector3(1, 1, 1);
			dialogue.transform.localScale = new Vector3(0, 0, 0);
		} else {
			inventory.transform.localScale = new Vector3(0, 0, 0);
			dialogue.transform.localScale = new Vector3(1, 1, 1);
		}
	}

	public Font label_font;
	public void OnGUI () {
		Vector3 pos = Input.mousePosition;
		GUI.skin.font = label_font;
		GUI.Label (new Rect (pos.x + 8, Screen.height - 16 - pos.y, Screen.width, 32), display_string);
	}

	string display_string = "";
	public void UpdateText(string str){
		display_string = str;
	}

	public void inventory_update () {
		for (int i = 0; i < 12; i++) {
			item_GUI [i].ChangeImage (Player_Manager.Instance.items [i] != Item_state.free);
		}
	}

	public bool inventory_item_hover(int num)
	{
		if (Player_Manager.Instance.items [num] == Item_state.inventory) {
			switch (num){
			case 0:
				UpdateText("Hair Pin");
				return true;

			case 1:
				UpdateText("Toy");
				return true;

			case 2:
				UpdateText("Food");
				return true;

			case 3:
				UpdateText("Coat");
				return true;

			case 4:
				UpdateText("Sickle");
				return true;

			case 5:
				UpdateText("Bamboo");
				return true;

			case 6:
				UpdateText("Book");
				return true;

			case 7:
				UpdateText("Sake");
				return true;
			}
		}
		return false;
	}

	public void Change_Line (Line l){
		Dialogue_Printer printer = dialogue.GetComponent <Dialogue_Printer> ();
		printer.Print (l);
	}

	public void register_gui_item (Inventory_Item i){
		item_GUI [i.inventory_item_number] = i;
	}

	//Singleton Pattern
	private static GUI_Manager instance = null;
	public static GUI_Manager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new GUI_Manager();
			}
			return instance;
		}
	}
}