using UnityEngine;

public class Mandelbox : App  {

	private float scale = 2;
	private int iterations = 20;
	private bool julia;
	private Vector3 c = Vector3.zero;
	private float mix;
	private Vector3 color = new Vector3(200, 200, 200);
	
	private void Start() {
		cameraType = CameraType.Orbit;
		maxR = 20;
		sens = 4;
	}
	
	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		shader.SetFloat("Scale", scale);
		shader.SetInt("Iterations", iterations);
		shader.SetBool("Julia", julia);
		shader.SetVector("C", c);
		shader.SetFloat("Mix", mix);
		shader.SetVector("Color", color);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
	}
	
	// options
	
	public float O_Scale {
		get => scale;
		set => scale = value;
	}
	
	public float O_Iterations {
		get => iterations;
		set => iterations = Mathf.RoundToInt(value);
	}
	
	public bool O_Julia {
		get => julia;
		set => julia = value;
	}

	public Vector3 O_C {
		get => c;
		set => c = value;
	}
	
	public float O_Mix {
		get => mix;
		set => mix = value;
	}

	public Vector3 O_Color {
		get => color;
		set => color = value;
	}
}
