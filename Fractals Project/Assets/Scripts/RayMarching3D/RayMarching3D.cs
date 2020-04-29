using System;
using UnityEngine;

public class RayMarching3D : App {

	private float altitude = 30;
	private float azimuth = 70;
	
	private void Start() {
		cameraType = CameraType.Free;
	}

	// main render method
	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		shader.SetVector("Sun", SunToVector().normalized);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);
	}

	private Vector3 SunToVector() {
		float x = Mathf.Cos(ToRadians(azimuth)) * Mathf.Cos(ToRadians(altitude));
		float y = Mathf.Sin(ToRadians(altitude));
		float z = Mathf.Sin(ToRadians(azimuth)) * Mathf.Cos(ToRadians(altitude));
		return -new Vector3(x, y, z);
	}

	private float ToRadians(float angle) {
		return angle * Mathf.PI / 180;
	}
	
	// options
	
	public float O_Altitude {
		get => altitude;
		set => altitude = value;
	}
	
	public float O_Azimuth {
		get => azimuth;
		set => azimuth = value;
	}
}