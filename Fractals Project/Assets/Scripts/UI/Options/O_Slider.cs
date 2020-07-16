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
	public TMP_InputField inputField;
	private bool typing;
	private bool ready;

	private void Start() {
		slider.maxValue = max;
		slider.minValue = min;
		slider.wholeNumbers = wholeNumbers;
		float value = (float) app.GetOption(optionName);
		slider.value = value;
		inputField.text = value.ToString();

		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		inputField.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		ready = true;
	}
	
	private void OnGUI() {
		float value = (float) app.GetOption(optionName);
		slider.value = value;
		if (!typing) {
			inputField.text = value.ToString();
		}
	}

	public void OnValueChanged(float val) {
		if (ready) {
			app.SetOption(optionName, val);
		}
	}
	
	public void OnEndEdit(string val) {
		if (ready) {
			app.SetOption(optionName, Mathf.Clamp(float.Parse(val), slider.minValue, slider.maxValue));
		}
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}
