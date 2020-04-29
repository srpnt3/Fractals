using UnityEngine;

public class InfiniteSpheres : App {

	// variables
	private float radius = 1;
	private bool repeat = false;
	private bool invert= false;

	private void Start() {
		cameraType = CameraType.Free;
	}

	// main render method
	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetFloat("Radius", radius);
		shader.SetBool("Repeat", repeat);
		shader.SetBool("Invert", invert);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);
	}
	
	// options

	public float O_Radius {
		get => radius;
		set => radius = value;
	}
	
	public bool O_Repeat {
		get => repeat;
		set => repeat = value;
	}
	
	public bool O_Invert {
		get => invert;
		set => invert = value;
	}
}