using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayMarching2D : App {

	private Vector2 origin;
	private float direction;
	private int steps = 30;
	private ComputeBuffer shapes;
	private List<Shape> objects;
	private List<Shape> visuals = new List<Shape>();

	// start
	private void Start() {
		
		// set origin
		origin = new Vector2(w / 2f, h / 2f);
		
		// create some random objects
		objects = new List<Shape>();
		float s = Mathf.Sqrt(w * h);
		for (int i = 0; i < 10; i++) {
			float s1 = Random.Range(s / 50, s / 10); // width
			float s2 = Random.Range(s / 50, s / 10); // height
			float s3 = Mathf.Max(s1, s2) + s / 1000; // margin
			objects.Add(new Shape(
				new Vector2(Random.Range(s3, w - s3), Random.Range(s3, h - s3)),
				new Vector2(s1, s2),
				Random.Range(0, 2),
				0,
				new Vector3(204, 51, 51)
			));
		}
	}

	// main render method
	protected override void Render(RenderTexture s) {
		
		March();
		InitShapes();

		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetBuffer(0, "Shapes", shapes);

		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
		
		// dispose buffer
		shapes.Dispose();
	}

	// get the smallest distance
	private float Min(Vector2 pos) {
		float d = w * h;
		foreach (Shape shape in objects) {
			d = Mathf.Min(d, GetDistance(pos, shape));
		}
		visuals.Add(new Shape(pos, new Vector2(Mathf.Abs(d), 0), 0, 1, new Vector3(246, 246, 246)));
		return d;
	}

	// march n times until the ray hits an object (d gets smaller than 1)
	private void March() {
		Vector2 pos = origin;
		for (int i = 0; i < steps; i++) {
			float d = Mathf.Abs(Min(pos));
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
	
	/*
	 * distance estimators
	 */
	
	private float GetDistance(Vector2 pos, Shape shape) {
		return shape.type == 0 ? DECircle(pos, shape.pos, shape.size[0]) : DESquare(pos, shape.pos, shape.size);
	}
	
	private static float Length(Vector2 a) {
		return Mathf.Sqrt(a.x * a.x + a.y * a.y);
	}

	private float DECircle(Vector2 c, Vector2 pos, float radius) {
		return Length(pos - c) - radius;
	}

	// from https://iquilezles.org/www/articles/distfunctions2d/distfunctions2d.htm
	private float DESquare(Vector2 c, Vector2 pos, Vector2 size) {
		Vector2 d = Abs(pos - c) - size;
		return Length(Vector2.Max(d, new Vector2(0, 0))) + Mathf.Min(Mathf.Max(d[0], d[1]), 0);
	}

	// abs of a vector
	private static Vector2 Abs(Vector2 x) {
		return new Vector2(Mathf.Abs(x[0]), Mathf.Abs(x[1]));
	}
	
	/*
	 * controls
	 */

	private bool posSet;
	private new void Update() {
		base.Update();
		
		if (controls.dragging) {
			if (posSet) {
				Vector2 d = origin - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				if (steps < 1) steps = 1;
				if (steps > 100) steps = 100;
				direction = getAngle(d);
			} else {
				posSet = true;
				origin = Input.mousePosition;
			}
			ReRender();
		} else if (posSet) {
			posSet = false;
		}
	}

	// vector angle
	private static float getAngle(Vector2 p) {
		float a = Vector2.SignedAngle(Vector2.left, p);
		if (a < 0) {
			a += 360;
		}
		return a;
	}

	// shape struct
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
	
	/*
	 * options
	 */

	public Vector2 O_Position {
		get => origin;
		set => origin = value;
	}
	
	public float O_Direction {
		get => direction;
		set => direction = value;
	}
	
	public float O_Steps {
		get => steps;
		set => steps = Mathf.RoundToInt(value);
	}
}