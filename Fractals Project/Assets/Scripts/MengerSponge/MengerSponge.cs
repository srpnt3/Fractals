﻿using UnityEngine;

public class MengerSponge : App {
	
	private int iterations;
	private float size = 1;
	private float edge = 1;
	private bool repeat;
	
	private void Start() {
		cameraType = CameraType.Orbit;
	}
	
	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		shader.SetInt("Iterations", iterations);
		shader.SetFloat("Size", size);
		shader.SetFloat("Edge", edge);
		shader.SetBool("Repeat", repeat);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
	}
	
	/*
	 * options
	 */
	
	public float O_Iterations {
		get => iterations;
		set => iterations = Mathf.RoundToInt(value);
	}
	
	public float O_Size {
		get => size;
		set => size = value;
	}
	
	public float O_Edge {
		get => edge;
		set => edge = value;
	}
	
	public bool O_Repeat {
		get => repeat;
		set {
			if (value) {
				cameraType = CameraType.Free;
			} else {
				cameraType = CameraType.Orbit;
				transform.LookAt(Vector3.zero);
				transform.position = transform.position.normalized * Mathf.Clamp(transform.position.magnitude, 1, 5);
				ReRender();
			}
			repeat = value;
		}
	}
}
