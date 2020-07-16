using UnityEngine;

public class OctahedronFlake : App {

	private int iterations;
	private float size = 1;
	private Vector3 offset = new Vector3(0, 1, 0);
	private float sizeDec = 2;
	
	private void Start() { cameraType = CameraType.Orbit; }
	
	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		shader.SetInt("Iterations", iterations);
		shader.SetFloat("Size", size);
		shader.SetVector("Offset", offset);
		shader.SetFloat("SizeDec", sizeDec);
		
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
	
	public Vector3 O_Offset {
		get => offset;
		set => offset = value;
	}
	
	public float O_SizeDec {
		get => sizeDec;
		set => sizeDec = value;
	}
}
