using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mandelbrot : App {
	
	public float zoom = 1;
	public Vector2 center = new Vector2(-0.5F, 0);
	[Range(10, 1000)] public int iterations = 100;

	private Vector4 area; // re, im, width, height
	private Vector3[] colorGradient;

	private void Start() {
		colorGradient = Gradients.CreateGradient(new Vector4[] {
			new Vector4(29, 31, 42, 0f),
			new Vector4(204, 51, 51, 1f),
		}, 10);
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		
		Init();
		CalculateArea();
		
		// variables
		iterations = iterations / 20 * 20;
		ComputeBuffer colors = new ComputeBuffer(colorGradient.Length, sizeof(float) * 3);
		colors.SetData(colorGradient);

		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture (0, "Source", s);
		shader.SetVector("Area", area); // re, im, width, height
		shader.SetInt("Iterations", iterations);
		shader.SetBuffer(0, "Colors", colors);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);

		// apply
		Graphics.Blit(tex, d);
		
		// Dispose buffers
		colors.Dispose();
	}

	// calculate the viewed area
	private void CalculateArea() {
		float width = 4 / zoom;
		float height = 4 / zoom;
		float r = (float) w / h;

		if (width / height < r) {
			// Screen wider than area
			width = height / h * w;
		} else if (width / height > r) {
			// Screen higher than area
			height = width / w * h;
		}

		float x = center.x - (width / 2);
		float y = center.y + (height / 2);
		area = new Vector4(x, y, width, height);
	}
}