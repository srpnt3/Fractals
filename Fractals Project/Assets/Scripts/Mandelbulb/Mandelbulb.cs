using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Mandelbulb : App {

	private float power = 11;
	private int steps = 100;
	private int iterations = 8;
	private float mix = 0.5f;
	
	private void Start() {
		cameraType = CameraType.Orbit;
		sens = 10;
	}
	
	// main render method
	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		shader.SetFloat("Power", power);
		shader.SetInt("Steps", steps);
		shader.SetInt("Iterations", iterations);
		shader.SetFloat("Mix", mix);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);
	}
	
	// options
	
	public float O_Power {
		get => power;
		set => power = value;
	}
	
	public float O_Steps {
		get => steps;
		set => steps = Mathf.RoundToInt(value);
	}
	
	public float O_Iterations {
		get => iterations;
		set => iterations = Mathf.RoundToInt(value);
	}
	
	public float O_Mix {
		get => mix;
		set => mix = value;
	}
}
