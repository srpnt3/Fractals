using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffect : MonoBehaviour {

	public float scale = 1;
	public bool inverted = false;
	public RectTransform rectTransform;

	private Vector2 center;
	private Vector2 pos;
	private Vector2 d;
	private Vector2 aPos;

	void Start() {
		aPos = rectTransform.anchoredPosition;
	}

	void Update() {
		center = new Vector2(Screen.width / 2f, Screen.height / 2f);
		pos = Input.mousePosition;
		d = new Vector2((pos.x - center.x) / 40 * scale, (pos.y - center.y) / 40 * scale);
		rectTransform.anchoredPosition = inverted ? aPos + d : aPos - d;
	}
}
