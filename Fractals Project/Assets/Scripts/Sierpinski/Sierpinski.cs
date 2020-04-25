using UnityEngine;

public class Sierpinski : App {

	// variables
	private int n = 3;
	private int iterations = 5;

	// main render method
	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture (0, "Source", s);
		shader.SetInt("N", n);
		shader.SetInt("Iterations", iterations);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);
	}
	
	// options

	public float O_N {
		get => n;
		set => n = Mathf.RoundToInt(value);
	}
	
	public float O_Iterations {
		get => iterations;
		set => iterations = Mathf.RoundToInt(value);
	}
}
