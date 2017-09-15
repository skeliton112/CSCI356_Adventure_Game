using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour {

	public Camera normal_camera;
	public Shader normal_shader;

	public Material mat;

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		normal_camera.RenderWithShader (normal_shader, "");
		Graphics.Blit(src, dest, mat);
	}
}
