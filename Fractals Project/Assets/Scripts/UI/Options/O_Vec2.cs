using TMPro;
using UnityEngine;

public class O_Vec2 : MonoBehaviour {
	
	public App app;
	public string optionName;

	// included in prefab
	public TMP_InputField inputFieldA;
	public TMP_InputField inputFieldB;
	private bool typing;
	private bool ready;

	private void Start() {
		Vector2 value = (Vector2) app.GetOption(optionName);
		inputFieldA.text = value.x.ToString();
		inputFieldB.text = value.y.ToString();

		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		inputFieldA.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		inputFieldB.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		ready = true;
	}

	private void Update() {
		if (!typing) {
			Vector2 value = (Vector2) app.GetOption(optionName);
			inputFieldA.text = value.x.ToString();
			inputFieldB.text = value.y.ToString();
		}
	}

	public void OnEndEdit() {
		if (ready) {
			app.SetOption(optionName, new Vector2(float.Parse(inputFieldA.text), float.Parse(inputFieldB.text)));
		}
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}