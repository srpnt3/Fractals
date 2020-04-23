﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffect : MonoBehaviour {

	public float scale = 1;
	public bool inverted = false;

	private RectTransform rt;
	private Vector2 center;
	private Vector2 pos;
	private Vector2 d;
	private Vector2 aPos;

	void Start() {
		rt = GetComponent<RectTransform>();
		aPos = rt.anchoredPosition;
	}

	/*void Update() {
		center = new Vector2(Screen.width / 2, Screen.height / 2);
		pos = Input.mousePosition;
		d = new Vector2((pos.x - center.x) / 40 * scale, (pos.y - center.y) / 40 * scale);
		rt.anchoredPosition = inverted ? aPos + d : aPos - d;
	}*/

	private void Update() {
		center = new Vector2(Screen.width / 2f, Screen.height / 2f);
		pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		d = (pos - center) / (Screen.width / 48f) * scale;
		rt.anchoredPosition = inverted ? aPos + d : aPos - d;
	}
}
