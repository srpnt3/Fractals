using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour {

	public GameObject up;
	public GameObject down;
	public ScrollRect scrollRect;
	public Color disabledColor;

	private Color upC;
	private Color downC;

	private void Start() {
		upC = up.GetComponent<Graphic>().color;
		downC = down.GetComponent<Graphic>().color;
	}

	private void Update() {
		float x = scrollRect.verticalNormalizedPosition;
		if (Math.Abs(x - 1) < 0.05f) {
			if (up.GetComponent<Graphic>().color == upC) StartCoroutine(Fade(up, upC, disabledColor));
		} else if (Math.Abs(x) < 0.05f) {
			if (down.GetComponent<Graphic>().color == downC) StartCoroutine(Fade(down, downC, disabledColor));
		} else {
			if (up.GetComponent<Graphic>().color == disabledColor) StartCoroutine(Fade(up, disabledColor, upC));
			if (down.GetComponent<Graphic>().color == disabledColor) StartCoroutine(Fade(down, disabledColor, downC));
		}
	}
	
	IEnumerator Fade(GameObject obj, Color a, Color b) {
		for (int i = 0; i <= 10; i++) {
			obj.GetComponent<Graphic>().color = Color.Lerp(a, b, i / 10f);
			yield return new WaitForSeconds(0.01f);
		}
	}
}
