using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class onHoverPlay : MonoBehaviour, IPointerEnterHandler{


	public Renderer rend1; // play game
	public Renderer rend2; // load game
	//	public Renderer rend3; // audio
	//	public Renderer rend4; // quit

	public void OnPointerEnter(PointerEventData eventData)
	{


		rend1.enabled = true; // play
		rend2.enabled = false; // load 
		//	rend3.enabled = false; // audio
		//	rend4.enabled = false; // quit



	}
}
