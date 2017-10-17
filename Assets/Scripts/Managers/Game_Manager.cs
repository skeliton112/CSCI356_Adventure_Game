using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

public delegate void Conversation_Callback (int state);

public delegate void Animation_Caller (Character character, string name);

public class Game_Manager {

	float squared (float s){return s * s;}
	public static event Animation_Caller Animation_Play;

	public void Update () { //Update loop

		if (Input.GetMouseButtonDown (0) && game_state == Game_state.talking)
			Advance_Conversation ();

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (menu_up)
				is_paused = false;
			else
				is_paused = true;
			menu_up = !menu_up;
		}


		if (load_locked) {
			loadScene ();
		} else if (target_scene != "" || transition_timer != 0) {
			
			if (target_scene != "") {
				
				transition_timer += Time.deltaTime;

				if (transition_timer >= 1) {
					transition_timer = 1;
					load_locked = true;
				}

			} else {
				transition_timer -= Time.deltaTime;
				if (transition_timer <= 0) {
					transition_timer = 0;
					is_paused = false;
				}
			}
		}
	}

	//Scene change
	float transition_timer = 0; bool load_locked = false;
	public float fade_amount {
		get {
			float t = 1 - (0 > transition_timer ? 0 : 1 < transition_timer ? 1 : transition_timer);
			return 1 - t*t;
		}
	}

	Material post_effect; string target_scene = "";
	Vector2 position; Vector3 player_position; float aspect = (Camera.main.pixelWidth / (float)Camera.main.pixelHeight); float radius = 0;

	public void ChangeScene (string scene_name, Vector3 target) {
		position = Camera.main.WorldToViewportPoint (Player_Manager.Instance.position + Vector3.up * 0.5f);
		player_position = target;
		radius = squared (Mathf.Max (aspect * position.x, aspect * (1 - position.x))) + squared (Mathf.Max (position.y, 1 - position.y));

		target_scene = scene_name;
		is_paused = true;
	}
	public Vector4 transition_effect {
		get { return new Vector4 (position.x, position.y, radius, aspect); }
	}
	AsyncOperation async;
	void loadScene(){
		if (target_scene != "") {
			async = SceneManager.LoadSceneAsync (target_scene, LoadSceneMode.Single);
			target_scene = "";
			async.allowSceneActivation = false;
		} else if (async.progress < 0.9f) {
			
		} else {
			transition_timer = 1.25f;
			load_locked = false;
			async.allowSceneActivation = true;
			position = Camera.main.WorldToViewportPoint (player_position + Vector3.up * 0.5f);
			Player_Manager.Instance.set_init_position (player_position);
		}
	}

	//Time wrapper
	private bool is_paused;
	public static float deltaTime {
		get {
			return Instance.is_paused ? 0 : Time.deltaTime;
		}
	}

	//Pause menu
	public bool menu_up = false;

	//Selection System
	Item item; public bool has_selection;
	public Item selected_item {
		get { return item; }
	}
	public void Select_Item (Item i){
		item = i;
		has_selection = true;
		GUI_Manager.Instance.inventory_update ();
	}
	public void Deselect_Item () {
		has_selection = false;
		GUI_Manager.Instance.inventory_update ();
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
		if (current_line < conversation.lines.Length) {
			GUI_Manager.Instance.Change_Line (conversation.lines [current_line]);
			if (Animation_Play != null)
				Animation_Play (conversation.lines [current_line].speaker, conversation.lines [current_line].speaker_animation);
		} else
			End_Conversation ();
	}
	private void End_Conversation () {
		game_state = Game_state.walking;
		if (conversation.inventory_change != null) {
			Player_Manager.Instance.item_change (conversation.inventory_change);
			Debug.Log (conversation.inventory_change.item);
		}
		conversation_callback (conversation.state_change);
	}

	//Save/Load
	public void Save () {
		
        System.Xml.Serialization.XmlSerializer writer =   
            new System.Xml.Serialization.XmlSerializer(typeof(Item_state[]));  

        string path =  "Assets/XML/items_save.xml";  
        System.IO.FileStream file = System.IO.File.Create(path);  

        writer.Serialize(file, Player_Manager.Instance.items);  
        file.Close();

		writer = new System.Xml.Serialization.XmlSerializer(typeof(CharacterPair[]));
		path = "Assets/XML/character_states_saves.xml";
		file = System.IO.File.Create(path);
		
		writer.Serialize(file, Character_Manager.Instance.states.Select(kv=>new CharacterPair(){id = kv.Key, value=kv.Value}).ToArray());

		file.Close();
	}
	
	public void Load () {
		Debug.Log("Load");
		string path = "Assets/XML/items_save.xml";

		XmlSerializer serializer = new XmlSerializer(typeof(Item_state[]));

		StreamReader reader = new StreamReader(path);
		Player_Manager.Instance.items = (Item_state[])serializer.Deserialize(reader);

		reader.Close();

		serializer = new System.Xml.Serialization.XmlSerializer(typeof(CharacterPair[]));
		path = "Assets/XML/character_states_saves.xml";
		reader = new StreamReader(path);
		
		Character_Manager.Instance.states = ((CharacterPair[])serializer.Deserialize(reader)).ToDictionary(i => i.id, i => i.value);

		reader.Close();
	}
	
	public void PrintStates()
	{
		string text = "";
		for(int i = 0; i < 12; i++)
		{
			text += (Player_Manager.Instance.items[i]+", ");
		}
		Debug.Log(text);
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
