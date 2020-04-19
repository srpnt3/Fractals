using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class InfiniteSpheres : App {

	// variables
	[Header("Options")]
	[Range(0, 3)] public float radius;
	[Range(0, 1)] public float fogStrength;
	public bool repeat;
	public bool invert;
	public bool outline;

	private void Start() {
		cameraType = CameraType.Free;
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		
		// init functions
		Init();

		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetFloat("Radius", radius);
		shader.SetFloat("FogStrength", fogStrength);
		shader.SetBool("Repeat", repeat);
		shader.SetBool("Invert", invert);
		shader.SetBool("Outline", outline);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);

		// apply the texture
		Graphics.Blit(tex, d);
	}
}