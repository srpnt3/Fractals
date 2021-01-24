using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class O_Slider : MonoBehaviour, IPointerClickHandler {
	
	[Header("General")]
	public App app;
	public string optionName;
	public float min;
	public float max;
	public bool wholeNumbers;
	
	[Header("Animation")]
	public bool animate;
	public float animationSpeed;
	public bool oscillating;
    
	// included in prefab
	[Header("Do not change")]
	public Slider slider;
	public TMP_InputField inputField;
	private bool typing;
	private bool ready;
	private bool animating;

	private void Start() {
		slider.maxValue = max;
		slider.minValue = min;
		slider.wholeNumbers = wholeNumbers;
		float value = (float) app.om.GetOption(optionName);
		slider.value = value;
		inputField.text = value.ToString();

		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		inputField.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		ready = true;
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (animate && eventData.button == PointerEventData.InputButton.Right) {
			if (animating) {
				app.om.StopOptionAnimation(optionName);
				slider.interactable = true;
				inputField.interactable = true;
			} else {
				app.om.StartOptionAnimation(optionName, animationSpeed, min, max, oscillating);
				slider.interactable = false;
				inputField.interactable = false;
			}
			animating = !animating;
		}
	}

	private void OnGUI() {
		float value = (float) app.om.GetOption(optionName);
		slider.value = value;
		if (!typing) {
			inputField.text = value.ToString();
		}
	}

	public void OnValueChanged(float val) {
		if (ready) {
			app.om.SetOption(optionName, val);
		}
	}
	
	public void OnEndEdit(string val) {
		if (ready) {
			app.om.SetOption(optionName, Mathf.Clamp(float.Parse(val), slider.minValue, slider.maxValue));
		}
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}
