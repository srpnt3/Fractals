using UnityEngine;

public class InfiniteSpheres : App {

	// variables
	private float radius = 1;
	private float fogStrength = 1;
	private bool repeat = false;
	private bool invert= false;
	private bool outline = false;

	private void Start() {
		cameraType = CameraType.Free;
	}

	// main render method
	protected override void Render(RenderTexture s) {
		
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
	}
	
	// options
	
	public void Radius(float v) { ReRender(); radius = v; }
	public void Fog(float v) { ReRender(); fogStrength = v; }
	public void Repeat(bool v) { ReRender(); repeat = v; }
	public void Invert(bool v) { ReRender(); invert = v; }
}