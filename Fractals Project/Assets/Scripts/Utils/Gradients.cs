using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Gradients {

	// create a gradient from two or more colors (c)
	// c in rgb (0-255) return in rgb (0-1). c must be in order with the percentages
	public static Vector3[] CreateGradient(Vector4[] c, int steps) {
		List<Colors.LCHColor> colors = new List<Colors.LCHColor>();

		if (steps < c.Length) {
			Debug.LogError("not enough steps");
			throw new Exception();
		}
		
		// loop through c
		for (int i = 0; i < c.Length; i++) {
			
			// convert and add the color
			Colors.LCHColor a = RGBtoLCH(Vec4toRGB(c[i]));
			colors.Add(a);

			// create a gradient between this color and the next one (if there is a next one)
			if (i < c.Length - 1) {
				Colors.LCHColor b = RGBtoLCH(Vec4toRGB(c[i + 1]));

				int s = Mathf.CeilToInt(c[i + 1][3] * steps) - Mathf.FloorToInt(c[i][3] * steps) - 2;
				if (s > 0) {
					List<Colors.LCHColor> g = new List<Colors.LCHColor>(LCHGradient(a, b, s));

					// add the gradient
					colors = colors.Concat(g).ToList();
				}
			}
		}

		// fix some bugs
		if (colors.Count < steps) {
			int error = steps - colors.Count;
			for (int e = 0; e < error; e++) {
				colors.Add(RGBtoLCH(Vec4toRGB(c[c.Length - 1])));
			}
		}

		// convert back to RGB
		Vector3[] ret = new Vector3[steps];

		for (int j = 0; j < steps; j++) {
			ret[j] = LCHtoRGB(colors[j]).ToVec3();
		}

		return ret;

		// create LCH gradients
		// returns gradient excluding start and end (s is the steps BETWEEN those colors)
		Colors.LCHColor[] LCHGradient(Colors.LCHColor a, Colors.LCHColor b, int s) {
			Colors.LCHColor[] result = new Colors.LCHColor[s];

			float deltaH;
			float deltaL = (b.L - a.L) / (s + 1);
			float deltaC = (b.C - a.C) / (s + 1);

			// H is in degrees so deltaH will be calculated differently
			if (Mathf.Abs(b.H - a.H) <= 180f) {
				deltaH = (b.H - a.H) / (s + 1f);
			} else {
				deltaH = -(360f - Mathf.Abs(b.H - a.H)) / (s + 1);
				deltaH *= Mathf.Sign(b.H - a.H);  // Sign function so that H will go around the right way
			}

			// calculate and add the colors
			for (int i = 0; i < s; i++) {
				result[i] = new Colors.LCHColor(
					a.L + deltaL * (i + 1f),
					a.C + deltaC * (i + 1f),
					a.H + deltaH * (i + 1f)
				);
				
				// adjust H
				if (result[i].H >= 360f) result[i].H = result[i].H - 360f;
				if (result[i].H < 0) result[i].H = result[i].H + 360f;
			}
			
			return result;
		}

		// some conversion functions
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
}