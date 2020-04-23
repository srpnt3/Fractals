using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderHelper : MonoBehaviour {

	public TextMeshProUGUI value;

	private void Start() {
		value.text = GetComponent<Slider>().value.ToString();
	}

	public void OnValueChanged(float val) {
		value.text = val.ToString();
	}
}
