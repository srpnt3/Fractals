using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
	
	public static SceneLoader instance;
	private Color clr;

	private void Awake() {
		clr = transform.GetChild(0).GetComponent<Graphic>().color;
		instance = this;
		StartCoroutine(Fade(false, () => { }));
	}

	public IEnumerator Fade(bool inout, Action callback) {
		Color a = clr, b = clr;
		a.a = inout ? 0 : 1;
		b.a = inout ? 1 : 0;
		
		for (int i = 0; i <= 20; i++) {
			transform.GetChild(0).GetComponent<Graphic>().color = Color.Lerp(a, b, i / 20f);
			yield return new WaitForSeconds(0.01f);
		}
		callback();
	}

	private void Load(int i) {
		StartCoroutine(Fade(true, () => {
			SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
			SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive).completed += o => {
				SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(i));
				StartCoroutine(Fade(false, () => { }));
			};
		}));
	}

	public static void LoadByName(string n) {
		try {
			instance.Load((int) Enum.Parse(typeof(SceneNames), n));
		} catch (Exception e) {
			Console.WriteLine(e);
		}
	}

	public static void LoadByIndex(int i) {
		instance.Load(i);
	}

	public enum SceneNames {
		MainMenu = 0,
		Loading = 1,
		Mandelbrot = 2,
		Sierpinski = 3,
		RayMarching2D = 4,
		RayMarching3D = 5,
		InfiniteSpheres = 6,
		Mandelbulb = 7,
		MengerSponge = 8,
		OctahedronFlake = 9,
		Mandelbox = 10,
		Audio = 11,
	}
}