using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	
	public Graphic targetGraphic;
	public Color normal;
	public Color hover;

	public void OnPointerEnter(PointerEventData eventData) {
		StartCoroutine(Fade(normal, hover));
	}

	public void OnPointerExit(PointerEventData eventData) {
		StartCoroutine(Fade(hover, normal));
	}
	
	IEnumerator Fade(Color a, Color b) {
		for (int i = 0; i <= 10; i++) {
			targetGraphic.color = Color.Lerp(a, b, i / 10f);
			yield return new WaitForSeconds(0.01f);
		}
	}
}
