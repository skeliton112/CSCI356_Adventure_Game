using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour {

	public Camera normal_camera;
	public Shader normal_shader;

	public Material mat;

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		normal_camera.RenderWithShader (normal_shader, "");

		mat.SetFloat ("_FadeAmount", 1 - Game_Manager.Instance.fade_amount);
		mat.SetVector ("_CircleCentre", Game_Manager.Instance.transition_effect);

		Graphics.Blit(src, dest, mat);
	}
}
