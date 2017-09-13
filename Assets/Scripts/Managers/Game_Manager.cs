using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void Conversation_Callback (int state);

public class Game_Manager {

	public void Update () { //Update loop
		if (Input.GetMouseButtonDown (0) && game_state == Game_state.talking)
			Advance_Conversation ();
	}

	//Time wrapper
	private bool is_paused;
	public static float deltaTime {
		get {
			return Instance.is_paused ? 0 : Time.deltaTime;
		}
	}

	//Selection System
	Item item; public bool has_selection;
	public Item selected_item {
		get { return item; }
	}
	public void Select_Item (Item i){
		item = i;
		has_selection = true;
	}
	public void Deselect_Item () {
		has_selection = false;
	}

	//Input wrapper
	public bool WantsToMove (){
		return !EventSystem.current.IsPointerOverGameObject() && game_state == Game_state.walking && Input.GetMouseButtonDown (0);
	}
	public bool WantsToUse (){
		return !EventSystem.current.IsPointerOverGameObject() && !has_selection && game_state == Game_state.walking && Input.GetMouseButtonDown (1);
	}
	public bool WantsToUseXOn (){
		return !EventSystem.current.IsPointerOverGameObject() && has_selection && game_state == Game_state.walking && Input.GetMouseButtonDown (1);
	}

	//Conversation Runner
	Game_state game_state = Game_state.walking;
	public Game_state State {
		get {
			return game_state;
		}
	}
	int current_line; Conversation conversation;
	Conversation_Callback conversation_callback;
	public void Start_Conversation (Conversation c, Conversation_Callback cb) {
		conversation = c;
		conversation_callback = cb;
		game_state = Game_state.talking;
		current_line = -1;
		Deselect_Item ();
		Advance_Conversation ();
	}
	private void Advance_Conversation () {
		current_line++;
		if (current_line < conversation.lines.Length)
			GUI_Manager.Instance.Change_Line (conversation.lines [current_line]);
		else
			End_Conversation ();
	}
	private void End_Conversation () {
		game_state = Game_state.walking;
		if (conversation.inventory_change != null)
			Player_Manager.Instance.item_change (conversation.inventory_change);
		conversation_callback (conversation.state_change);
	}

	//Save/Load
	public void Save () {

	}
	public void Load () {

	}

	//Singleton pattern
	private static Game_Manager instance = null;
	public static Game_Manager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new Game_Manager();
			}
			return instance;
		}
	}
}
