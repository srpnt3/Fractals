using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sierpinski : App {

	[Range(1, 4)] public int n = 3;
	[Range(1, 9)] public int iterations = 5;
	
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		
		Init();
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture (0, "Source", s);
		shader.SetInt("N", n);
		shader.SetInt("Iterations", iterations);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);
		
		// apply
		Graphics.Blit(tex, d);
	}
}
