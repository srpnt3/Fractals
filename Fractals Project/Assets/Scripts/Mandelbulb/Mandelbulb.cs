using UnityEngine;

public class Mandelbulb : App {
	
	private float power = 11;
	private int iterations = 10;
	private bool julia;
	private Vector3 c = Vector3.zero;
	private float mix = 0.5f;
	private float eyeOffset = -0.1f;
	private bool crossEye = false;
	
	private void Start() { cameraType = CameraType.Orbit; }

	// main render method
	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		shader.SetFloat("Power", power);
		shader.SetInt("Iterations", iterations);
		shader.SetBool("Julia", julia);
		shader.SetVector("C", c);
		shader.SetFloat("Mix", mix);
		shader.SetBool("CrossEye", crossEye);
		if (crossEye) SetEyes(); // calculate left and right eye for 3D effect
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);

		void SetEyes() {
			
			// left eye
			cam.transform.Translate(new Vector3(-eyeOffset, 0));
			shader.SetMatrix("CamToWorldL", cam.cameraToWorldMatrix);
			shader.SetMatrix("CamInverseProjectionL", cam.projectionMatrix.inverse);
		
			// right eye
			cam.transform.Translate(new Vector3(eyeOffset * 2, 0));
			shader.SetMatrix("CamToWorldR", cam.cameraToWorldMatrix);
			shader.SetMatrix("CamInverseProjectionR", cam.projectionMatrix.inverse);
			
			// reset camera
			cam.transform.Translate(new Vector3(-eyeOffset, 0));
		}
	}
	
	/*
	 * options
	 */
	
	public float O_Power {
		get => power;
		set => power = value;
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
}
