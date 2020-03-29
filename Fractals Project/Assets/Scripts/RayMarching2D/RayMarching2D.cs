using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayMarching2D : App {
	
	// variables
	[Header("Options")]
	public Vector2 origin = new Vector2(960, 540);
	[Range(0, 360)] public float direction = 0;
	[Range(1, 100)] public int steps = 1;

	// lists of the objects
	private ComputeBuffer shapes;
	private List<Shape> objects;
	private List<Shape> visuals = new List<Shape>();

	// create some objects that look random
	private void Start() {
		objects = new List<Shape>();
		objects.Add(new Shape(new Vector2(1290, 451), new Vector2(69, 0), 0, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(684, 669), new Vector2(85, 0), 0, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(390, 281), new Vector2(89, 0), 0, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(410, 868), new Vector2(91, 65), 1, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(1260, 756), new Vector2(100, 100), 1, 0, new Vector3(204, 51, 51)));
		objects.Add(new Shape(new Vector2(1739, 180), new Vector2(137, 97), 1, 0, new Vector3(204, 51, 51)));
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		
		// init functions
		Init();
		March();
		InitShapes();

		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetBuffer(0, "Shapes", shapes);

		shader.Dispatch(0, Mathf.CeilToInt(w / 8), Mathf.CeilToInt(h / 8), 1);

		// apply the texture
		Graphics.Blit(tex, d);

		// dispose buffers
		shapes.Dispose();
	}

	// get the smallest distance
	private float Min(Vector2 pos) {
		int d = Int32.MaxValue;
		foreach (Shape shape in objects) {
			float x = GetDistance(pos, shape);
			if (x < d) d = Mathf.RoundToInt(x);
		}
		visuals.Add(new Shape(pos, new Vector2(d, 0), 0, 1, new Vector3(246, 246, 246)));
		return d;
	}

	// march n times until the ray hits an object (gets smaller than 1)
	private void March() {
		Vector2 pos = origin;
		for (int i = 0; i < steps; i++) {
			float d = Min(pos);
			pos = new Vector2(
				pos.x + d * Mathf.Cos(direction * (Mathf.PI / 180)),
				pos.y + d * Mathf.Sin(direction * (Mathf.PI / 180))
			);
			if (d < 1 || d > 1000) break;
		}
		visuals.Add(new Shape(origin, new Vector3(pos.x, pos.y, 1), 2, 0, new Vector3(246, 246, 246)));
	}

	// create the shape buffer
	private void InitShapes() {
	
		visuals.Add(new Shape(origin, new Vector2(5, 0), 0, 0, new Vector3(246, 246, 246)));
		List<Shape> allShapes = objects.Concat(visuals).ToList();

		shapes = new ComputeBuffer(allShapes.Count, sizeof(float) * 8 + sizeof(int) * 2);
		shapes.SetData(allShapes);
		
		// reset list
		visuals = new List<Shape>();
	}
	
	// distance estimators
	private float GetDistance(Vector2 pos, Shape shape) {
		return shape.type == 0 ? DECircle(pos, shape.pos, shape.size[0]) : DESquare(pos, shape.pos, shape.size);
	}
	
	private float Length(Vector2 a) {
		return Mathf.Sqrt(a.x * a.x + a.y * a.y);
	}

	private float DECircle(Vector2 c, Vector2 pos, float radius) {
		return Length(pos - c) - radius;
	}

	private float DESquare(Vector2 c, Vector2 pos, Vector2 size) {
		Vector2 d = Abs(pos - c) - size;
		return Length(Vector2.Max(d, new Vector2(0, 0))) + Mathf.Min(Mathf.Max(d[0], d[1]), 0);
	}

	private Vector2 Abs(Vector2 x) {
		return new Vector2(Mathf.Abs(x[0]), Mathf.Abs(x[1]));
	}

	// the shape object
	struct Shape {
		public Vector2 pos;
		public Vector3 size;
		public int type; // 0: sphere, 1: cube, 2: line
		public int border; // 0: no, 1: yes
		public Vector3 color;

		public Shape(Vector2 pos, Vector3 size, int type, int border, Vector3 color) {
			this.pos = pos;
			this.size = size;
			this.type = type;
			this.border = border;
			this.color = color;
		}
	}
}