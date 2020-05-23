using System;
using UnityEngine;

public class RayMarching3D : App {

	private float elevation = 30;
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
		shader.SetVector("Sun", -SphericalCoordsToCartesianCoords(azimuth, elevation));
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
	}
	
	// options
	
	public float O_Elevation {
		get => elevation;
		set => elevation = value;
	}
	
	public float O_Azimuth {
		get => azimuth;
		set => azimuth = value;
	}
}