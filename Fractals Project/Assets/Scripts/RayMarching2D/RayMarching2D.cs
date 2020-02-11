using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class RayMarching2D : App {
	
	public Vector2 origin = new Vector2(960, 540);
	[Range(0, 360)] public float direction;
	[Range(1, 100)] public int steps;

	private ComputeBuffer shapes;
	private List<Shape> objects;
	private List<Shape> visuals = new List<Shape>();

	private void Start() {
		objects = new List<Shape>(); // Looks Random
		objects.Add(new Shape(new Vector2(1290, 451), 69, 0, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(684, 669), 85, 0, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(390, 281), 89, 0, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(410, 868), 91, 1, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(1260, 756), 100, 1, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(1739, 180), 137, 1, 0, new Vector3(204, 51, 51)));
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		
		Init();
		//March();
		Min(origin);
		InitShapes();

		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetBuffer(0, "Shapes", shapes);

		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);

		// apply
		Graphics.Blit(tex, d);

		// Dispose buffers
		shapes.Dispose();
	}

	private float Min(Vector2 pos) {
		int d = 1000000;
		foreach (Shape shape in objects) {
			float x = GetDistance(pos, shape);
			if (x < d) d = Mathf.RoundToInt(x);
		}
		visuals.Add(new Shape(pos, d, 0, 1, new Vector3(246, 246, 246)));
		return d;
	}

	/*private void March() {
		
	}*/

	private void InitShapes() {
	
		visuals.Add(new Shape(origin, 5, 0, 0, new Vector3(246, 246, 246)));
		List<Shape> allShapes = objects.Concat(visuals).ToList();

		shapes = new ComputeBuffer(allShapes.Count, sizeof(float) * 5 + sizeof(int) * 3);
		shapes.SetData(allShapes);
		
		// reset List
		visuals = new List<Shape>();
	}
	
	// Distance Estimators

	private float GetDistance(Vector2 pos, Shape shape) {
		return shape.type == 0 ? DECircle(pos, shape.pos, shape.size) : DESquare(pos, shape.pos, shape.size);
	}
	
	private float Length(Vector2 a) {
		return Mathf.Sqrt(a.x * a.x + a.y * a.y);
	}

	private float DECircle(Vector2 c, Vector2 pos, float radius) {
		return Length(pos - c) - radius;
	}

	private float DESquare(Vector2 c, Vector2 pos, float size) {
		Vector2 d = Abs(pos - c) - new Vector2(size, size);
		return Length(Vector2.Max(d, new Vector2(0, 0))) + Mathf.Min(Mathf.Max(d[0], d[1]), 0);
	}

	private Vector2 Abs(Vector2 x) {
		return new Vector2(Mathf.Abs(x[0]), Mathf.Abs(x[1]));
	}

	struct Shape {
		public Vector2 pos;
		public int size;
		public int type; // 0: Sphere, 1: Cube
		public int border; // 0: no, 1: yes
		public Vector3 color;

		public Shape(Vector2 pos, int size, int type, int border, Vector3 color) {
			this.pos = pos;
			this.size = size;
			this.type = type;
			this.border = border;
			this.color = color;
		}
	}
}