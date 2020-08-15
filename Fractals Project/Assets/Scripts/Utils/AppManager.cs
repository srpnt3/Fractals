using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour {

	private List<App> apps = new List<App>();
	
	public GameObject content;
	public GameObject list;
	
	private GameObject infos;
	private GameObject load;
	private TextMeshProUGUI title;
	private TextMeshProUGUI text;
	private TextMeshProUGUI type;
	private TextMeshProUGUI size;
	private TextMeshProUGUI date;

	private int s;

	public int Selected {
		get => s;
		set {
			s = value;
			UpdateContent();
		}
	}

	public void Awake() {
		
		// Load the _Loading scene if necessary
		if (SceneManager.sceneCount == 1)
			SceneManager.LoadSceneAsync((int) SceneLoader.SceneNames.Loading, LoadSceneMode.Additive);
	}

	public void Start() {

		// initialize all children
		infos = content.transform.GetChild(3).gameObject;
		load = content.transform.GetChild(4).gameObject;
		title = content.transform.GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		text = content.transform.GetChild(2).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
		type = content.transform.GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		size = content.transform.GetChild(3).GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
		date = content.transform.GetChild(3).GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
		
		// load apps from file
		apps = JsonConvert.DeserializeObject<List<App>>(Resources.Load("Apps").ToString());
		
		for (int i = 1; i < apps.Count; i++) {
			CreateElement(i);
		}

		// scale and position the list
		int h = apps.Count * 24 + (apps.Count - 1) * 50;
		GameObject elements = list.transform.GetChild(1).GetChild(0).gameObject;
		elements.GetComponent<RectTransform>().sizeDelta = new Vector2(0, h);
		elements.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, h / -2.0f);

		// update the content
		UpdateContent();
	}

	// create a menu element for the app
	private void CreateElement(int i) {
		GameObject e = Instantiate(Resources.Load("Link") as GameObject, list.transform.GetChild(1).GetChild(0));
		e.name = apps[i].Name;
		e.GetComponent<TextMeshProUGUI>().text = "// " + apps[i].Name;
		e.GetComponent<Button>().transition = Selectable.Transition.None;
		e.GetComponent<Button>().onClick.AddListener(delegate { Selected = i; });
	}

	// update name, description...
	private void UpdateContent() {
		
		App a = apps[Selected];
		title.text = a.Name;
		text.text = a.Description;
		
		// home selected
		if (Selected == 0) { // disable unnecessary objects
			infos.SetActive(false);
			load.SetActive(false);
			return;
		}
		
		// enable objects
		infos.SetActive(true);
		load.SetActive(true);
		
		// update the content
		type.text = a.Type ? "3D" : "2D";
		size.text = (Math.Round(a.Size / 10f) / 100f).ToString("0.00") + " KB";
		date.text = a.Date;
	}

	// load the scene currently selected
	public void LoadSelectedScene() {
		SceneLoader.LoadByName(apps[Selected].Name.Replace(" ", string.Empty).Replace(".", string.Empty));
	}

	[Serializable]
	private class App {
		public string Name { get; }
		public string Description { get; }
		public bool Type { get; }
		public short Size { get; }
		public string Date { get; }

		public App(string name, bool type, short size, string date, string description) {
			Name = name;
			Description = description;
			Type = type;
			Size = size;
			Date = date;
		}
	}

	public void Quit() {
		Application.Quit();
	}
}