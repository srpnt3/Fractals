using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HoverAssist : MonoBehaviour {
	public Color normal;
	public Color hover;
	
	public void Enter() {
		StartCoroutine(fade(normal, hover));
	}

	public void Exit() {
		StartCoroutine(fade(hover, normal));
	}
	
	IEnumerator fade(Color a, Color b) {
		for (int i = 0; i < 10; i++) {
			GetComponent<Graphic>().color = Color.Lerp(a, b, i / 10f);
			yield return new WaitForSeconds(0.01f);
		}
	}
}
