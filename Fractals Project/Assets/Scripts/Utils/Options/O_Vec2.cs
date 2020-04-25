using TMPro;
using UnityEngine;

public class O_Vec2 : MonoBehaviour {
	
	public App app;
	public string optionName;

	// included in prefab
	public TMP_InputField inputFieldA;
	public TMP_InputField inputFieldB;
	private bool typing;

	private void Start() {
		inputFieldA.text = ((Vector2) app.GetOption(optionName)).x.ToString();
		inputFieldB.text = ((Vector2) app.GetOption(optionName)).y.ToString();

		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		inputFieldA.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		inputFieldB.textComponent.alignment = TextAlignmentOptions.MidlineRight;
	}

	private void Update() {
		if (!typing) {
			inputFieldA.text = ((Vector2) app.GetOption(optionName)).x.ToString();
			inputFieldB.text = ((Vector2) app.GetOption(optionName)).y.ToString();
		}
	}

	public void OnValueChanged() {
		app.SetOption(optionName, new Vector2(float.Parse(inputFieldA.text), float.Parse(inputFieldB.text)));
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}