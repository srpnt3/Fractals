using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mandelbrot : App {
	
	private float zoom = 1;
	private Vector2 center = new Vector2(-0.5f, 0);
	private int iterations = 100;
	private bool julia;
	private Vector2 c;
	private Vector4 area; // (re, im, width, height)
	private Vector3[] colorGradient;

	// create the Gradient
	private void Start() {
		colorGradient = Gradients.CreateGradient(new[] {
			new Vector4(29, 31, 42, 0f),
			new Vector4(204, 51, 51, 1f),
		}, 10);
	}

	// main render method
	protected override void Render(RenderTexture s) {
		
		// calculate area and buffer
		CalculateArea();
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
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
		
		// dispose buffer
		colors.Dispose();
	}

	// calculate the viewed area
	private void CalculateArea() {
		float width = 4f / Mathf.Pow(1.1f, zoom);
		float height = 4f / Mathf.Pow(1.1f, zoom);
		float r = (float) w / h;

		if (width / height < r) // screen wider than area
			width = height / h * w;
		else if (width / height > r) // screen higher than area
			height = width / w * h;
		
		float x = center.x - width / 2f;
		float y = -center.y + height / 2f;
		area = new Vector4(x, y, width, height);
	}
	
	private Vector2 ConvertCoords(Vector2 coords) {
		return new Vector2(coords[0] / (w / area[2]) + area[0], coords[1] / (h / area[3]) - area[1]);
	}
	
	private new void Update() {
		base.Update();
		
		// update coordinates
		Vector2 coords = ConvertCoords(Input.mousePosition);
		canvas.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "C ( " + coords.x.ToString("0.0000000") + " / " + coords.y.ToString("0.0000000") + "i )";
		
		// controls
		if (Mathf.Abs(controls.deltaZoom) > 0) {
			zoom += controls.deltaZoom * Time.deltaTime;
			if (zoom < 1) zoom = 1;
			center = coords;
			CalculateArea();
			Vector2 d = center - ConvertCoords(Input.mousePosition);
			center += d;
			ReRender();	
		}

		if (controls.dragging) {
			center += ConvertCoords(controls.dragPosition) - ConvertCoords(Input.mousePosition);
			controls.dragPosition = Input.mousePosition;
			ReRender();
		}

		if (controls.rightClick) {
			controls.rightClick = false;
			if (!EventSystem.current.IsPointerOverGameObject()) {
				c = coords;
				julia = !julia;
				ReRender();
			}
		}
	}

	/*
	 * options
	 */

	public float O_Zoom {
		get => zoom;
		set => zoom = value;
	}

	public float O_Iterations {
		get => iterations;	
		//set => iterations = Mathf.RoundToInt(value / 20) * 20;
		set => iterations = Mathf.RoundToInt(value);
	}
	
	public bool O_Julia {
		get => julia;
		set => julia = value;
	}
	
	public Vector2 O_C {
		get => c;
		set => c = value;
	}
}