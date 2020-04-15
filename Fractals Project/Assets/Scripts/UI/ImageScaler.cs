using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScaler : MonoBehaviour {

	public float scale = 1f;

	private RectTransform rt;
	private RawImage img;
	private Vector2 size;

	void Start() {
		img = GetComponent<RawImage>();
		rt = img.rectTransform;
		size = new Vector2(img.mainTexture.width, img.mainTexture.height);
	}
	
	void Update() {
		double screenRatio = (double) Screen.width / Screen.height;
		double imageRatio = (double) size.x / size.y;
	
		// image scaling
		if (screenRatio < imageRatio) { // use height
			rt.sizeDelta = new Vector2(size.x * (Screen.height / size.y) * scale, Screen.height * scale);
		} else if (screenRatio > imageRatio) { // use width
			rt.sizeDelta = new Vector2(Screen.width * scale, size.y * (Screen.width / size.x) * scale);
		} else {
			rt.sizeDelta = new Vector2(Screen.width * scale, Screen.height * scale);
		}
	}
}
