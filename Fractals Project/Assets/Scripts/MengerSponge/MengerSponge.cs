using UnityEngine;

public class MengerSponge : App {
	
	private int iterations;
	private float size = 1;
	private float edge = 1;
	private float cut;
	private bool cantor;
	
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
		shader.SetFloat("Cut", cut);
		shader.SetBool("Cantor", cantor);
		
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
	
	public float O_Cut {
		get => cut;
		set => cut = value;
	}
	
	public bool O_Cantor {
		get => cantor;
		set => cantor = value;
	}
}
