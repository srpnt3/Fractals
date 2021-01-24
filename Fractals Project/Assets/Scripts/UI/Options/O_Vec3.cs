using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class O_Vec3 : MonoBehaviour, IPointerClickHandler {
	
	[Header("General")]
	public App app;
	public string optionName;

	[Header("Animation")]
	public bool animate;
	public Vector3 animationSpeed;
	public Vector3 min;
	public Vector3 max;
	public bool oscillating;
	
	// included in prefab
	[Header("Do not change")]
	public TMP_InputField inputFieldA;
	public TMP_InputField inputFieldB;
	public TMP_InputField inputFieldC;
	private bool typing;
	private bool ready;
	private bool animating;

	private void Start() {
		Vector3 value = (Vector3) app.om.GetOption(optionName);
		inputFieldA.text = value.x.ToString();
		inputFieldB.text = value.y.ToString();
		inputFieldC.text = value.z.ToString();

		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		inputFieldA.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		inputFieldB.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		inputFieldC.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		ready = true;
	}
	
	public void OnPointerClick(PointerEventData eventData) {
		if (animate && eventData.button == PointerEventData.InputButton.Right) {
			if (animating) {
				app.om.StopOptionAnimation(optionName);
				inputFieldA.interactable = true;
				inputFieldB.interactable = true;
				inputFieldC.interactable = true;
			} else {
				app.om.StartOptionAnimation(optionName, animationSpeed, min, max, oscillating);
				inputFieldA.interactable = false;
				inputFieldB.interactable = false;
				inputFieldC.interactable = false;
			}
			animating = !animating;
		}
	}


	private void OnGUI() {
		if (!typing) {
			Vector3 value = (Vector3) app.om.GetOption(optionName);
			inputFieldA.text = value.x.ToString();
			inputFieldB.text = value.y.ToString();
			inputFieldC.text = value.z.ToString();
		}
	}

	public void OnEndEdit() {
		if (ready) {
			app.om.SetOption(optionName, new Vector3(float.Parse(inputFieldA.text), float.Parse(inputFieldB.text), float.Parse(inputFieldC.text)));
		}
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}