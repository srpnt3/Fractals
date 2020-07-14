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
		
		// create all apps
		apps.Add(new App("Mandelbrot", false, 0, "02 FEB",
			"The Mandelbrot fractal and its Julia Sets." +
			"\n\nUse the mouse to move around and the scroll wheel to zoom."));
		apps.Add(new App("Sierpinski", false, 0, "11 FEB", "The Sierpinski Triangle and the Sierpinski Carpet. Both can be created using the same algorithm."));
		apps.Add(new App("Ray Marching 2D", false, 0, "11 FEB",
			"A 2D demonstration of Ray Marching." +
			"\n\nClick on the screen to set the starting position of the ray." +
			"\nDrag your mouse to control the direction in which the ray will march."));
		apps.Add(new App("Ray Marching 3D", true, 0, "28 MAR",
			"A simple 3D scene, which was rendered using Ray Marching. The scene contains different objects to showcase different features of Ray Marching." +
			"\n\nClick on the screen to toggle between a normal mouse and the camera controls." +
			"\nWhile in camera mode use the mouse to look around, WASD keys to move and Q and E to rotate the camera."));
		apps.Add(new App("Infinite Spheres", true, 0, "29 MAR",
			"A demonstration of the capabilities of Ray Marching. A single modification to the position causes the space to repeat and thus gives the illusion of infinite objects." +
			"\n\nClick on the screen to toggle between a normal mouse and the camera controls." +
			"\nWhile in camera mode use the mouse to look around, WASD keys to move and Q and E to rotate the camera."));
		apps.Add(new App("Mandelbulb", true, 0, "30 APR",
			"A 3D version of the Mandelbrot fractal." +
			"\n\nClick on the screen to toggle between a normal mouse and the camera controls." +
			"\nWhile in camera mode use the mouse to turn the fractal."));
		apps.Add(new App("Menger Sponge", true, 0, "03 MAY",
			"A 3D version of the Sierpinski Carpet." +
			"\n\nClick on the screen to toggle between a normal mouse and the camera controls." +
			"\nToggling the option REPEAT will switch between a free camera (same controls as Ray Marching 3D and Infinite Spheres) and an orbit camera (same controls as Mandelbulb)."));
		apps.Add(new App("Octahedron Flake", true, 0, "05 MAY",
			"A cooler version of the Sierpinski Triangle that consists of octahedrons instead of pyramids." +
			"\n\nClick on the screen to toggle between a normal mouse and the camera controls." +
			"\nWhile in camera mode use the mouse to turn the fractal."));
		apps.Add(new App("Mandelbox", true, 0, "17 MAY",
			"A box like 3D version of the Mandelbrot fractal." +
			"\n\nClick on the screen to toggle between a normal mouse and the camera controls." +
			"\nWhile in camera mode use the mouse to turn the fractal."));
		apps.Add(new App("Audio", true, 0, "29 JUN",
			"A Julia set of the Mandelbulb fractal that reacts to audio. The different frequencies control the size and C value of the fractal."));

		for (int i = 0; i < apps.Count; i++) {
			CreateElement(i);
		}

		// scale and position the list
		int h = (apps.Count + 1) * 24 + apps.Count * 50;
		GameObject elements = list.transform.GetChild(1).GetChild(0).gameObject;
		elements.GetComponent<RectTransform>().sizeDelta = new Vector2(0, h);
		elements.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, h / -2.0f);

		// update the content
		UpdateContent();
	}

	// create an element for the app
	private void CreateElement(int i) {
		GameObject e = Instantiate(Resources.Load("Link") as GameObject, list.transform.GetChild(1).GetChild(0));
		e.name = apps[i].Name;
		e.GetComponent<TextMeshProUGUI>().text = "// " + apps[i].Name;
		e.GetComponent<Button>().transition = Selectable.Transition.None;
		e.GetComponent<Button>().onClick.AddListener(delegate { Selected = i + 1; });
	}

	private void UpdateContent() {
		
		// home selected
		if (Selected == 0) {
			
			// disable unnecessary objects
			infos.SetActive(false);
			load.SetActive(false);

			// update the content
			title.text = "The Project";
			text.text = "This program is a collection of all the fractals, games and other little projects that I have programmed as part of my Matura Paper." + 
						"\nThe name of the paper is \"Fraktale und ihre Anwendung im Game-Design\". In it I analyze different fractals, explain how I implemented them and discuss different applications." +
						"\n" +
						"\nBy selecting one of the apps on the left a quick description, the controls and some additional info about the app will be displayed. An app can be loaded by clicking the LOAD button on the bottom right of the screen." +
						"\nBy pressing R inside an app the Options menu will be opened, in which the parameters and other setting of the app can be modified. The F12 button can be used to take a screenshot. To exit the app simply press ESCAPE or click the arrow on the top left of the screen." +
						"\n" +
						"\nI recommend having a decent graphics card to run this program unless you enjoy stop motion fractals." +
						"\n" +
						"\nThe source code of \"Fractals\" is available on GitHub: https://github.com/AutisticSlav/Fractals.";
			return;
		}
		
		// reenable objects
		infos.SetActive(true);
		load.SetActive(true);
		
		// update the content
		App a = apps[Selected - 1];
		title.text = a.Name;
		text.text = a.Description;
		type.text = a.Type ? "3D" : "2D";
		size.text = (Math.Round(a.Size / 10f) / 100f).ToString("0.00") + " KB";
		date.text = a.Date;
	}

	public void LoadSelectedScene() {
		SceneLoader.LoadByName(apps[Selected - 1].Name.Replace(" ", string.Empty));
	}

	private class App {
		public string Name { get; }
		public string Description { get; }
		public bool Type { get; }
		public short Size { get; }
		public string Date { get; }

		public App(string name, bool type, short size, string date, String description) {
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