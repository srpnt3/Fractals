using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class O_Vec2 : MonoBehaviour, IPointerClickHandler {
	
	[Header("General")]
	public App app;
	public string optionName;

	[Header("Animation")]
	public bool animate;
	public Vector2 animationSpeed;
	public Vector2 min;
	public Vector2 max;
	public bool oscillating;
	
	// included in prefab
	[Header("Do not change")]
	public TMP_InputField inputFieldA;
	public TMP_InputField inputFieldB;
	private bool typing;
	private bool ready;
	private bool animating;

	private void Start() {
		Vector2 value = (Vector2) app.om.GetOption(optionName);
		inputFieldA.text = value.x.ToString();
		inputFieldB.text = value.y.ToString();

		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		inputFieldA.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		inputFieldB.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		ready = true;
	}
	
	public void OnPointerClick(PointerEventData eventData) {
		if (animate && eventData.button == PointerEventData.InputButton.Right) {
			if (animating) {
				app.om.StopOptionAnimation(optionName);
				inputFieldA.interactable = true;
				inputFieldB.interactable = true;
			} else {
				app.om.StartOptionAnimation(optionName, animationSpeed, min, max, oscillating);
				inputFieldA.interactable = false;
				inputFieldB.interactable = false;
			}
			animating = !animating;
		}
	}

	private void OnGUI() {
		if (!typing) {
			Vector2 value = (Vector2) app.om.GetOption(optionName);
			inputFieldA.text = value.x.ToString();
			inputFieldB.text = value.y.ToString();
		}
	}

	public void OnEndEdit() {
		if (ready) {
			app.om.SetOption(optionName, new Vector2(float.Parse(inputFieldA.text), float.Parse(inputFieldB.text)));
		}
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}