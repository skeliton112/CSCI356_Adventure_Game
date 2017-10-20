using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void Interaction_Callback ();

public class Plan {
	public Interactable target;
	public int item_id;

	public Plan (Interactable t, int i){
		target = t;
		item_id = i;
	}

	public void Do () {
		target.Use (item_id);
	}
};

public class Player_Manager {

	Plan plan = null;
	Walker player = null;
	GameObject player_prefab;
	Vector3 init_position = Vector3.zero;
	public Item_state[] items = new Item_state[12];

	public Vector3 position {
		get {
			if (player != null)
				return player.position;
			else
				return Vector3.zero;
		}
	}
	public Vector3 direction {
		get {
			return player.direction;
		}
	}

	private Player_Manager () {
		SceneManager.sceneLoaded += OnLevelLoad;
	}

	void OnLevelLoad (Scene scene, LoadSceneMode mode) {
		if (player_prefab != null) {
			GameObject pl = GameObject.Instantiate (player_prefab, init_position, Quaternion.Euler (0,0,0));
			player = pl.GetComponent <Walker> ();
		}
	}

	public void item_change (Inventory_Change change) {
		items [(int)change.item] = change.type == Action_Type.give ? Item_state.inventory : Item_state.free;
		GUI_Manager.Instance.inventory_update ();
	}

	void Interact () {
		if (plan != null) {
			plan.Do ();
			plan = null;
		}
	}

	public void set_player (GameObject p){
		player_prefab = p;
		if (player == null) {
			GameObject pl = GameObject.Instantiate (player_prefab, Vector3.zero, Quaternion.Euler (0,0,0));
			player = pl.GetComponent <Walker> ();
		}
	}

	public void set_path (Vector3 target){
		plan = null;
		if (player != null)
			player.set_path (WalkSystem.Instance.get_path (player.position, target), Interact);
	}

	public void clear_path () {
		plan = null;
		player.clear_path ();
	}

	public void set_init_position (Vector3 p){
		init_position = p;
	}

	public void set_target (Interactable i, int item_id = -1){
		plan = new Plan (i, item_id);
	}

	//Singleton pattern
	private static Player_Manager instance = null;
	public static Player_Manager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new Player_Manager();
			}
			return instance;
		}
	}
}
