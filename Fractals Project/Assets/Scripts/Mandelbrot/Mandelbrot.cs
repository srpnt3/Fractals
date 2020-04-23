using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Mandelbrot : App {
	
	// variables
	private float zoom = 1;
	private Vector2 center = new Vector2(-0.5f, 0);
	private int iterations = 100;
	private bool julia = false;
	private Vector2 c = Vector2.zero;

	// private vars
	private Vector4 area; // (re, im, width, height)
	private Vector3[] colorGradient;

	// create the Gradient
	private void Start() {
		colorGradient = Gradients.CreateGradient(new Vector4[] {
			new Vector4(29, 31, 42, 0f),
			new Vector4(204, 51, 51, 1f),
		}, 10);
	}

	// main render method
	protected override void Render(RenderTexture s) {
		
		// calculate some things
		CalculateArea();
		iterations = iterations / 20 * 20;
		ComputeBuffer colors = new ComputeBuffer(colorGradient.Length, sizeof(float) * 3);
		colors.SetData(colorGradient);

		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture (0, "Source", s);
		shader.SetBool("Julia", julia);
		shader.SetVector("Area", area); // re, im, width, height
		shader.SetVector("C", c);
		shader.SetInt("Iterations", iterations);
		shader.SetBuffer(0, "Colors", colors);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);
		
		// dispose buffers
		colors.Dispose();
	}

	// calculate the viewed area
	private void CalculateArea() {
		float width = 4 / zoom;
		float height = 4 / zoom;
		float r = (float) w / h;

		if (width / height < r) // screen wider than area
			width = height / h * w;
		else if (width / height > r) // screen higher than area
			height = width / w * h;
		
		float x = center.x - (width / 2);
		float y = center.y + (height / 2);
		area = new Vector4(x, y, width, height);
	}
	
	// display coordinates
	
	public TextMeshProUGUI info;

	private Vector2 converCoords(Vector2 coord) {
		return new Vector2(coord[0] / (w / area[2]) + area[0], coord[1] / (h / area[3]) - area[1]);
	}
	
	private void Update() {
		Vector2 coord = converCoords(Input.mousePosition);
		info.text = "P ( " + coord.x + " / " + coord.y + "i )";
	}

	// options
	public void Zoom(string v) { ReRender(); zoom = float.Parse(v); }
	public void CoordinatesX(string v) { ReRender(); center.x = float.Parse(v); }
	public void CoordinatesY(string v) { ReRender(); center.y = float.Parse(v); }
	public void Iterations(float v) { ReRender(); iterations = Mathf.RoundToInt(v); }
	public void Julia(bool v) { ReRender(); julia = v; }
	public void CX(string v) { ReRender(); c.x = float.Parse(v); }
	public void CY(string v) { ReRender(); c.y = float.Parse(v); }
}