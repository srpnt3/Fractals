using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHelper : MonoBehaviour {

    public GameObject on;
    public GameObject off;

    public void Start() {
        on.SetActive(GetComponent<Toggle>().isOn);
        off.SetActive(!GetComponent<Toggle>().isOn);
    }

    public void Toggle() {
        on.SetActive(GetComponent<Toggle>().isOn);
        off.SetActive(!GetComponent<Toggle>().isOn);
    }
}
