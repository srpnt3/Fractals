using UnityEngine;

public class Sierpinski : App {

	private float n = 3;
	private int iterations = 5;

	protected override void Render(RenderTexture s) {
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture (0, "Source", s);
		shader.SetFloat("N", n);
		shader.SetInt("Iterations", iterations);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
	}
	
	/*
	 * options
	 */

	public float O_N {
		get => n;
		set => n = value;
	}
	
	public float O_Iterations {
		get => iterations;
		set => iterations = Mathf.RoundToInt(value);
	}
}