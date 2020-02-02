using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mandelbrot : MonoBehaviour {

	public ComputeShader shader;
	public float zoom = 1;
	public Vector2 center = new Vector2(-0.5F, 0);
	[Range (10, 1000)] public int iterations = 100;

	private Camera cam;
	private RenderTexture t;
	private int w;
	private int h;
	private Vector4 area; // re, im, width, height
	private ComputeBuffer colors;

	void Start() {
		//colorGradient = Gradients.BlackToRed;
		Vector3[]  colorGradient = Gradients.CreateGradient(new Vector4[] {
			new Vector4(29, 31, 42, 0f),
			new Vector4(204, 51, 51, 1f)
		}, 10);
		colors = new ComputeBuffer(colorGradient.Length, sizeof(float) * 3);
		colors.SetData(colorGradient);
	}

	void Update() {

	}

	void OnRenderImage(RenderTexture s, RenderTexture d) {

		// variables
		cam = Camera.current;
		w = cam.pixelWidth;
		h = cam.pixelHeight;
		iterations = iterations / 20 * 20;

		// area
		CalculateArea();

		// texture
		if (t == null || t.width != w || t.height != h) {
			if (t != null) {
				t.Release();
			}
			t = new RenderTexture(w, h, 32);
			t.enableRandomWrite = true;
			t.Create();
		}

		// shader
		shader.SetTexture(0, "Texture", t);
		shader.SetVector("Area", area); // re, im, width, height
		shader.SetInt("Iterations", iterations);
		shader.SetBuffer(0, "Colors", colors);
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);

		// apply
		Graphics.Blit(t, d);
	}

	// calculate the viewed area
	private void CalculateArea() {
		float width = 4 / zoom;
		float height = 4 / zoom;
		float r = (float)w / h;

		if (width / height < r) { // Screen wider than area
			width = height / h * w;
		} else if (width / height > r) { // Screen higher than area
			height = width / w * h;
		}

		float x = center.x - (width / 2);
		float y = center.y + (height / 2);
		area = new Vector4(x, y, width, height);
	}
}
