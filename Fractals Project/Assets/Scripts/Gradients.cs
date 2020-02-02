using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gradients {

	public static Vector3[] BlackToRed = new Vector3[] {
		new Vector3(29, 31, 42),
		new Vector3(39, 37, 58),
		new Vector3(55, 41, 72),
		new Vector3(76, 44, 83),
		new Vector3(99, 44, 89),
		new Vector3(124, 42, 91),
		new Vector3(148, 38, 88),
		new Vector3(170, 37, 80),
		new Vector3(189, 40, 68),
		new Vector3(204, 51, 51),
	};

	public static Vector3[] CreateGradient(Vector4[] Colors, int steps) {

		// test function
		ColorTest();

		// create vars
		Gradient g = new Gradient();
		GradientColorKey[] c = new GradientColorKey[Colors.Length];
		GradientAlphaKey[] a = new GradientAlphaKey[Colors.Length];

		// create keys
		for (int i = 0; i < Colors.Length; i++) {
			c[i].color = Colors[i];
			c[i].time = Colors[i][3];
			a[i].alpha = 1.0f;
			a[i].time = Colors[i][3];
		}

		g.SetKeys(c, a);

		Vector3[] r = new Vector3[steps];

		for (int i = 0; i < steps; i++) {
			float p = (float)i / (steps - 1);
			r[i] = (Vector4)g.Evaluate(p);
		}

		return r;
	}

	private static void ColorTest() {
		for (int i = 0; i < 10; i++) {
			Debug.Log(RGBtoLCH(Vec3toRGB(BlackToRed[i])).ToVec3().ToString("F6"));
		}
	}
	
	private static Colors.RGBColor Vec3toRGB(Vector3 v) {
		return new Colors.RGBColor(v[0] / 255, v[1] / 255, v[2] / 255);
	}

	private static Colors.LCHColor RGBtoLCH(Colors.RGBColor c) {
		return c.ToXYZ().ToLAB().ToLCH();
	}

	private static Colors.RGBColor LCHtoRGB(Colors.LCHColor c) {
		return c.ToLAB().ToXYZ().ToRGB();
	}
}
