using TMPro;
using UnityEngine;

public class O_Vec3 : MonoBehaviour {
	
	public App app;
	public string optionName;

	// included in prefab
	public TMP_InputField inputFieldA;
	public TMP_InputField inputFieldB;
	public TMP_InputField inputFieldC;
	private bool typing;
	private bool ready;

	private void Start() {
		Vector3 value = (Vector3) app.GetOption(optionName);
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

	private void OnGUI() {
		if (!typing) {
			Vector3 value = (Vector3) app.GetOption(optionName);
			inputFieldA.text = value.x.ToString();
			inputFieldB.text = value.y.ToString();
			inputFieldC.text = value.z.ToString();
		}
	}

	public void OnEndEdit() {
		if (ready) {
			app.SetOption(optionName, new Vector3(float.Parse(inputFieldA.text), float.Parse(inputFieldB.text), float.Parse(inputFieldC.text)));
		}
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}