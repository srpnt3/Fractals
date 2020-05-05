using TMPro;
using UnityEngine;

public class O_Single : MonoBehaviour {

	public App app;
	public string optionName;

	// included in prefab
	public TMP_InputField inputField;
	private bool typing;
	private bool ready;
	
	private void Start() {
		inputField.text = ((float) app.GetOption(optionName)).ToString();
        
		// design stuff
		transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
		inputField.textComponent.alignment = TextAlignmentOptions.MidlineRight;
		ready = true;
	}
	
	private void Update() {
		if (!typing) {
			inputField.text = ((float) app.GetOption(optionName)).ToString();
		}
	}

	public void OnEndEdit(string val) {
		if (ready) {
			app.SetOption(optionName, float.Parse(val));
		}
	}

	public void OnSelect() {
		typing = true;
	}

	public void OnDeselect() {
		typing = false;
	}
}
