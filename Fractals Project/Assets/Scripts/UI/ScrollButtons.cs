using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public Scroll scroll;
	public bool up;

	private bool pressed;

	private void Update() {
		if (pressed) {
			scroll.ButtonScroll(up);
		}
	}

	public void OnPointerDown(PointerEventData eventData) {
		pressed = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		pressed = false;
	}
}