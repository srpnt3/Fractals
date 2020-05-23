using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class O_Bool : MonoBehaviour {

    public App app;
    public string optionName;
    
    // included in prefab
    public Toggle toggle;
    public GameObject on;
    public GameObject off;
    private bool ready;

    private void Start() {
        toggle.isOn = (bool) app.GetOption(optionName);
        
        // design stuff
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        on.SetActive(toggle.isOn);
        off.SetActive(!toggle.isOn);
        ready = true;
    }

    private void OnGUI() {
        toggle.isOn = (bool) app.GetOption(optionName);
    }

    public void OnValueChanged(bool val) {
        if (ready) {
            app.SetOption(optionName, val);

            // design stuff
            on.SetActive(val);
            off.SetActive(!val);
        }
    }
}
