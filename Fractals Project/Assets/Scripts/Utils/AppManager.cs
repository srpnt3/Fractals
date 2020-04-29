using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour {

	private List<App> apps = new List<App>();
	
	[Range(0, 5)] public byte id;
	public GameObject content;
	public GameObject list;
	public GameObject prefab;
	
	private GameObject infos;
	private GameObject load;
	private TextMeshProUGUI title;
	private TextMeshProUGUI text;
	private TextMeshProUGUI type;
	private TextMeshProUGUI size;
	private TextMeshProUGUI date;

	private byte id_;
	
	public void Start() {

		// initialize all children
		infos = content.transform.GetChild(3).gameObject;
		load = content.transform.GetChild(4).gameObject;
		title = content.transform.GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		text = content.transform.GetChild(2).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
		type = content.transform.GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		size = content.transform.GetChild(3).GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
		date = content.transform.GetChild(3).GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
		
		// create all apps
		apps.Add(new App("Mandelbrot", false, 4640, "02 FEB", 1, 
			"Press R to open the options menu in which you can modify certain variables." +
			"\n\nUse the mouse to move around and the scroll wheel to zoom."));
		apps.Add(new App("Sierpinski", false, 1800, "11 FEB", 2, 
			"Press R to open the options menu in which you can modify certain variables."));
		apps.Add(new App("Ray Marching 2D", false, 6370, "11 FEB", 3, 
			"Press R to open the options menu in which you can modify certain variables." +
			"\n\nClick to set the starting position of the ray and drag the mouse to control the direction."));
		apps.Add(new App("Ray Marching 3D", true, 2710, "28 MAR", 4, 
			"Press R to open the options menu in which you can modify certain variables." +
			"\n\nClick to toggle between a normal mouse and the camera controls." +
			"\nWhile camera controls are active use the mouse to look around, WASD key to move and Q and E to rotate"));
		apps.Add(new App("Infinite Spheres", true, 4000, "29 MAR", 5, 
			"Press R to open the options menu in which you can modify certain variables." +
			"\n\nClick to toggle between a normal mouse and the camera controls." +
			"\nWhile camera controls are active use the mouse to look around, WASD key to move and Q and E to rotate"));

		for (int i = 0; i < apps.Count; i++) {
			CreateElement(i);
		}

		// scale and position the list
		int h = (apps.Count + 1) * 24 + apps.Count * 50 + 120;
		list.GetComponent<RectTransform>().sizeDelta = new Vector2(400, h);
		list.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, -130 - h / 2);

		// update the content
		UpdateContent();
		
		id_ = id;
	}
	
	public void Update() {
		if (id_ != id) {
			UpdateContent();
			id_ = id;
		}
	}

	// create an element for the app
	private void CreateElement(int i) {
		GameObject e = Instantiate(prefab, list.transform.GetChild(1));
		e.name = apps[i].Name;
		e.GetComponent<TextMeshProUGUI>().text = "// " + apps[i].Name;
		e.GetComponent<Button>().transition = Selectable.Transition.None;
		e.GetComponent<Button>().onClick.AddListener(delegate { SetSelected(i + 1); });
		e.GetComponent<EventTrigger>().triggers[0].callback.AddListener( delegate { e.GetComponent<HoverAssist>().Enter(); });
		e.GetComponent<EventTrigger>().triggers[1].callback.AddListener( delegate { e.GetComponent<HoverAssist>().Exit(); });
	}

	public void SetSelected(int i) {
		id = (byte) i;
	}
	
	private void UpdateContent() {
		
		// home selected
		if (id == 0) {
			
			// disable unnecessary objects
			infos.SetActive(false);
			load.SetActive(false);

			// update the content
			title.text = "The Project";
			text.text = "TODO: Write introduction";
			return;
		}
		
		// reenable objects
		infos.SetActive(true);
		load.SetActive(true);
		
		// update the content
		App a = apps[id - 1];
		title.text = a.Name;
		text.text = a.Description;
		type.text = a.Type ? "3D" : "2D";
		size.text = (Math.Round(a.Size / 10f) / 100f).ToString("0.00") + " KB";
		date.text = a.Date;
	}

	public void LoadSelectedScene() {
		GetComponent<SceneLoader>().Load(apps[id - 1].ID);
	}

	private class App {
		public string Name { get; }
		public string Description { get; }
		public bool Type { get; }
		public short Size { get; }
		public string Date { get; }
		public byte ID { get; }

		public App(string name, bool type, short size, string date, byte id, String description) {
			Name = name;
			Description = description; 
			Type = type;
			Size = size;
			Date = date;
			ID = id;
		}
	}

	public void Quit() {
		Application.Quit();
	}
}