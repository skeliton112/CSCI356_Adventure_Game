using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler  {
	public Sprite itemImage, greyImage;
	Sprite emptyImage;
	public int inventory_item_number;
	string label;
	bool active, hover;
	bool onButton;
	string text;
	int priority;
	
	// Use this for initialization
	void Start () {
		priority = 0;
		active = false;
		hover = false;
		GUI_Manager.Instance.register_gui_item (this);
		emptyImage = GetComponent<Image> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(1))
			priority = 2;
		if(Input.GetMouseButton(0))
			priority = 1;
	}

	public void OnPointerClick(PointerEventData eventData)
    {
		if (GUI_Manager.Instance.inventory_item_hover (inventory_item_number)) {
			Game_Manager.Instance.Select_Item ((Item)inventory_item_number);
		}
    }

	public void ChangeImage(bool isOn)
	{
		if (isOn) {
			if (Game_Manager.Instance.has_selection && Game_Manager.Instance.selected_item == (Item) inventory_item_number)
				GetComponent<Image> ().sprite = greyImage;
			else
				GetComponent<Image> ().sprite = itemImage;
		}else
			GetComponent<Image>().sprite = emptyImage;
	}

	public void OnPointerEnter(PointerEventData eventData)
    {
		GUI_Manager.Instance.inventory_item_hover(inventory_item_number);

    }

	public void OnPointerExit(PointerEventData eventData)
	{
		GUI_Manager.Instance.UpdateText("");
	}
}
