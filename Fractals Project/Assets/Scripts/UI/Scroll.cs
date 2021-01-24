using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour {

	public GameObject up, down;
	public ScrollRect scrollRect;
	public Color disabledColor;

	private Graphic upG, downG;
	private HoverEffect upH, downH;
	private Color upC, downC;

	private void Start() {
		upG = up.GetComponent<Graphic>();
		downG = down.GetComponent<Graphic>();
		upH = up.GetComponent<HoverEffect>();
		downH = down.GetComponent<HoverEffect>();
		upC = upG.color;
		downC = downG.color;
	}

	private void Update() {
		float x = scrollRect.verticalNormalizedPosition;
		if (Math.Abs(x - 1) < 0.05f) {
			upH.enabled = false;
			if (upG.color != disabledColor) StartCoroutine(Fade(upG, upC, disabledColor));
		} else if (Math.Abs(x) < 0.05f) {
			downH.enabled = false;
			if (downG.color != disabledColor) StartCoroutine(Fade(downG, downC, disabledColor));
		} else {
			upH.enabled = true;
			downH.enabled = true;
			if (upG.color == disabledColor) StartCoroutine(Fade(upG, disabledColor, upC));
			if (downG.color == disabledColor) StartCoroutine(Fade(downG, disabledColor, downC));
		}
	}

	public void ButtonScroll(bool dir) {
		scrollRect.verticalNormalizedPosition += dir ? Time.deltaTime : -Time.deltaTime;
	}

	IEnumerator Fade(Graphic g, Color a, Color b) {
		for (int i = 0; i <= 10; i++) {
			g.color = Color.Lerp(a, b, i / 10f);
			yield return new WaitForSeconds(0.01f);
		}
	}
}
