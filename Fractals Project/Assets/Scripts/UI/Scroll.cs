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
			up.GetComponent<HoverEffect>().enabled = false;
			if (up.GetComponent<Graphic>().color != disabledColor) StartCoroutine(Fade(up, upC, disabledColor));
		} else if (Math.Abs(x) < 0.05f) {
			down.GetComponent<HoverEffect>().enabled = false;
			if (down.GetComponent<Graphic>().color != disabledColor) StartCoroutine(Fade(down, downC, disabledColor));
		} else {
			up.GetComponent<HoverEffect>().enabled = true;
			down.GetComponent<HoverEffect>().enabled = true;
			if (up.GetComponent<Graphic>().color == disabledColor) StartCoroutine(Fade(up, disabledColor, upC));
			if (down.GetComponent<Graphic>().color == disabledColor) StartCoroutine(Fade(down, disabledColor, downC));
		}
	}

	public void ButtonScroll(bool up) {
		scrollRect.verticalNormalizedPosition += up ? Time.deltaTime : -Time.deltaTime;
	}

	IEnumerator Fade(GameObject obj, Color a, Color b) {
		for (int i = 0; i <= 10; i++) {
			obj.GetComponent<Graphic>().color = Color.Lerp(a, b, i / 10f);
			yield return new WaitForSeconds(0.01f);
		}
	}
}
