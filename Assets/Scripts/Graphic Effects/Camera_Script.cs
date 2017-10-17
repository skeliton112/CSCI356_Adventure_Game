using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Script : MonoBehaviour {
	
	public Material mat;

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		
		mat.SetFloat ("_FadeAmount", 1 - Game_Manager.Instance.fade_amount);
		mat.SetVector ("_CircleCentre", Game_Manager.Instance.transition_effect);

		Graphics.Blit(src, dest, mat);
	}
}
