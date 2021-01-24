using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class O_Single : MonoBehaviour, IPointerClickHandler {

	[Header("General")]
	public App app;
	public string optionName;

	[Header("Animation")]
	public bool animate;
	public float animationSpeed;
	public float min;
	public float max;
	public bool oscillating;
	
	// included in prefab
	[Header("Do not change")]
	public TMP_InputField inputField;
	private bool typing;
	private bool ready;
	private bool animating;
	
	private void Start() {
		inputField.text = ((float) app.om.GetOption(optionName)).ToString();
        
		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		inputField.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		ready = true;
	}
	
	public void OnPointerClick(PointerEventData eventData) {
		if (animate && eventData.button == PointerEventData.InputButton.Right) {
			if (animating) {
				app.om.StopOptionAnimation(optionName);
				inputField.interactable = true;
			} else {
				app.om.StartOptionAnimation(optionName, animationSpeed, min, max, oscillating);
				inputField.interactable = false;
			}
			animating = !animating;
		}
	}
	
	private void OnGUI() {
		if (!typing) {
			inputField.text = ((float) app.om.GetOption(optionName)).ToString();
		}
	}

	public void OnEndEdit(string val) {
		if (ready) {
			app.om.SetOption(optionName, float.Parse(val));
		}
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}
