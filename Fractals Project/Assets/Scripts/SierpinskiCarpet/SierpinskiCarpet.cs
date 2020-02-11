using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierpinskiCarpet : App {
	
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		
		Init();
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture (0, "Source", s);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);
		
		// apply
		Graphics.Blit(tex, d);
	}
}
