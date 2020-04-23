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
	
	public void N(float v) { ReRender(); n = Mathf.RoundToInt(v); }
	public void Iterations(float v) { ReRender(); iterations = Mathf.RoundToInt(v); }
}
