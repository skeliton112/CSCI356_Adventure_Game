using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[System.Serializable()]
public struct Line {
	public Character speaker;
	public string text;
}

[System.Serializable()]
public class Inventory_Change {
	public Action_Type type;
	public Item item;
}

[System.Serializable()]
public class Conversation {
	public Line[] lines;
	public int state_change;
	public Inventory_Change inventory_change;
}

[System.Serializable()]
public class Dialogue {
	public int state;
	public Conversation NoItem;
	public Conversation Default;
	public Conversation[] Items;
}

[System.Serializable()]
public class DialogueTree {
	public Dialogue[] dialogues;
}

public class Dialogue_Manager
{

	private Dictionary<string, DialogueTree> trees;

	private Dialogue_Manager() {
		trees = new Dictionary<string, DialogueTree> ();
	}

	public static void register (string key) {
		if (Instance.trees.ContainsKey (key))
			return;

		string path = "Assets/XML/" + key + ".xml";

		XmlSerializer serializer = new XmlSerializer(typeof(DialogueTree));

		StreamReader reader = new StreamReader(path);
		Instance.trees[key] = (DialogueTree)serializer.Deserialize(reader);
		reader.Close();
	}

	public Conversation this[string key, int state, int item_id] {
		get {
			if (!trees.ContainsKey (key))
				return null;
			foreach (Dialogue d in trees[key].dialogues) {
				if (d.state == state) {
					if (item_id < 0 || item_id > 7) {
						return d.NoItem;
					} else if (d.Items [item_id] == null) {
						return d.Default;
					} else {
						return d.Items [item_id];
					}
				}
			}
			return null;
		}
	}

	//Singleton pattern
	private static Dialogue_Manager instance = null;
	public static Dialogue_Manager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new Dialogue_Manager();
			}
			return instance;
		}
	}
}
