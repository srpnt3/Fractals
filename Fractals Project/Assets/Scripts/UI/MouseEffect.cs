using UnityEngine;

public class MouseEffect : MonoBehaviour {

	public float scale = 1;
	public bool inverted;

	private RectTransform rt;
	private Vector2 center;
	private Vector2 pos;
	private Vector2 d;
	private Vector2 aPos;

	void Start() {
		rt = GetComponent<RectTransform>();
		aPos = rt.anchoredPosition;
	}

	private void Update() {
		center = new Vector2(Screen.width / 2f, Screen.height / 2f);
		pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		d = (pos - center) / (Screen.width / 48f) * scale;
		rt.anchoredPosition = inverted ? aPos + d : aPos - d;
	}
}
