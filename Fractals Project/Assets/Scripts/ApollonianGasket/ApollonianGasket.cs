using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApollonianGasket : App {
	
	[Range(3, 6)] public int mainCircles;
	[Range(1, 100)] public int iterations;

	private int n;
	private float a;
	private float r;
	private ComputeBuffer circleBuffer;

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		
		Init();
		InitVars();

		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetBuffer(0, "Circles", circleBuffer);
		shader.SetInt("Iterations", iterations);
		shader.SetInt("N", n);
		shader.SetFloat("A", a);
		shader.SetFloat("R", r);

		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);

		// apply
		Graphics.Blit(tex, d);
		
		// Dispose buffers
		circleBuffer.Dispose();
	}

	private void InitVars() {
		n = mainCircles;
		a = Mathf.PI * 2f / n;
		r = Mathf.Sin(a) / Mathf.Sin((Mathf.PI - a) / 2f) / 2f;

		Circle[] circles = new Circle[n + 1];
		circles[0] = new Circle(new Vector2(0, 0), 1 - r);
		for (int i = 0; i < n; i++) {
			circles[i + 1] = new Circle(new Vector2(
					Mathf.Cos(a * i),
					Mathf.Sin(a * i)),
				r);
		}
		
		circleBuffer = new ComputeBuffer(circles.Length, sizeof(float) * 3);
		circleBuffer.SetData(circles);
	}

	struct Circle {
		public Vector2 pos;
		public float r;

		public Circle(Vector2 pos, float r) {
			this.pos = pos;
			this.r = r;
		}
	}
}