using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sobel : MonoBehaviour {

	public Material mat;
	public Material combiner;

	Camera cam;
	int width, height;

	void Start () {
		cam = GetComponent<Camera> ();
		width = cam.pixelWidth;
		height = cam.pixelHeight;

		cam.targetTexture = new RenderTexture (width, height, 0);
		combiner.SetTexture ("_OutlineTex", cam.targetTexture);
	}

	void Update () {
		if (width != cam.pixelWidth || height != cam.pixelHeight) {
			width = cam.pixelWidth;
			height = cam.pixelHeight;

			cam.targetTexture.Release ();
			cam.targetTexture = new RenderTexture (width, height, 0);
			combiner.SetTexture ("_OutlineTex", cam.targetTexture);

			Debug.Log ("Trigger");
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, mat);
	}
}
