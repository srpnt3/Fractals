using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public static class Gradients {

	// c in rgb (0-255) return in rgb (0-1). c must be in order with the percentages
	public static Vector3[] CreateGradient(Vector4[] c, int steps) {
		List<Colors.LCHColor> colors = new List<Colors.LCHColor>();

		for (int i = 0; i < c.Length; i++) {
			Colors.LCHColor a = RGBtoLCH(Vec4toRGB(c[i]));
			colors.Add(a);

			if (i < c.Length - 1) {
				Colors.LCHColor b = RGBtoLCH(Vec4toRGB(c[i + 1]));

				int s = Mathf.FloorToInt(c[i + 1][3] * steps) - Mathf.FloorToInt(c[i][3] * steps) - 2;
				List<Colors.LCHColor> lerp = new List<Colors.LCHColor>(LerpLCH(a, b, s));

				colors = colors.Concat(lerp).ToList();
			}
		}

		Vector3[] ret = new Vector3[steps];

		for (int j = 0; j < steps; j++) {
			ret[j] = LCHtoRGB(colors[j]).ToVec3();
		}

		return ret;

		// lerp (excluding start and end (also in steps))
		Colors.LCHColor[] LerpLCH(Colors.LCHColor a, Colors.LCHColor b, int s) {
			float deltaH;
			Colors.LCHColor[] result = new Colors.LCHColor[s];

			float deltaL = (b.L - a.L) / (s + 1);
			float deltaC = (b.C - a.C) / (s + 1);

			if (Mathf.Abs(b.H - a.H) <= 180f) {
				deltaH = (b.H - a.H) / (s + 1f);
			} else {
				deltaH = -(360f - Mathf.Abs(b.H - a.H)) / (s + 1);
				deltaH *= Math.Sign(b.H - a.H);  // sign function for the right sign
			}

			for (int i = 0; i < s; i++) {
				result[i] = new Colors.LCHColor(
					a.L + deltaL * (i + 1f),
					a.C + deltaC * (i + 1f),
					a.H + deltaH * (i + 1f)
				);
				if (result[i].H >= 360f) result[i].H = result[i].H - 360f;
				if (result[i].H < 0) result[i].H = result[i].H + 360f;
			}

			return result;
		}

		Colors.RGBColor Vec4toRGB(Vector4 v) {
			return new Colors.RGBColor(v[0] / 255f, v[1] / 255f, v[2] / 255f);
		}

		Colors.LCHColor RGBtoLCH(Colors.RGBColor col) {
			return col.ToXYZ().ToLAB().ToLCH();
		}

		Colors.RGBColor LCHtoRGB(Colors.LCHColor col) {
			return col.ToLAB().ToXYZ().ToRGB();
		}
	}

	/*public static Vector3[] CreateGradient(Vector4[] Colors, int steps) {

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
	}*/

	/*private static void ColorTest() {
	    for (int i = 0; i < 10; i++) {
	        Debug.Log(RGBtoLCH(Vec3toRGB(BlackToRed[i])).ToVec3().ToString("F6"));
	    }
	}*/

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