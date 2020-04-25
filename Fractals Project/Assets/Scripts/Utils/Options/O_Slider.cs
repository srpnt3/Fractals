using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class O_Slider : MonoBehaviour {
	
	public App app;
	public string optionName;
	public float min;
	public float max;
	public bool wholeNumbers;
    
	// included in prefab
	public Slider slider;
	public TextMeshProUGUI value;

	private void Start() {
		slider.minValue = min;
		slider.maxValue = max;
		slider.wholeNumbers = wholeNumbers;
		slider.value = (float) app.GetOption(optionName);

		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		value.text = slider.value.ToString();
	}
	
	private void Update() {
		slider.value = (float) app.GetOption(optionName);
	}

	public void OnValueChanged(float val) {
		app.SetOption(optionName, val);

		// design stuff
		value.text = val.ToString();
	}
}
