using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class App : MonoBehaviour {
	
	// variables
	public ComputeShader shader;
	public GameObject canvas;
	protected RenderTexture tex;
	protected RenderTexture tex2; // second render texture (serves different purposes)
	protected Camera cam;
	protected int w;
	protected int h;
	
	// controls
	public readonly ControlsHelper controls;
	public CameraType cameraType = CameraType.None;

	// rerender
	private bool update;
	public void ReRender() { update = true; }

	// render function
	protected abstract void Render(RenderTexture s);
	
	protected App() { controls = new ControlsHelper(this); }
	
	// awake
	protected void Awake() {
		
		// Load the _Loading scene if necessary
		if (SceneManager.sceneCount == 1)
			SceneManager.LoadSceneAsync((int) SceneLoader.SceneNames.Loading, LoadSceneMode.Additive);

		// register the controls
		controls.RegisterControls();
		
		// back button
		canvas.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(() => SceneLoader.LoadByIndex((int) SceneLoader.SceneNames.MainMenu));
	}

	// enable and disable
	protected void OnEnable() { controls.Enable(); cam = GetComponent<Camera>(); InitRenderTexture(); }
	protected void OnDisable() { controls.Disable(); }

	// update
	protected void Update () {
		controls.Update();
		
		canvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(1.0f / Time.smoothDeltaTime) + " FPS";
	}

	// on render image
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		InitRenderTexture();
		if (update) {
			Render(s);
			update = false;
		}
		Graphics.Blit(tex, d);
	}

	// init the render texture
	private void InitRenderTexture() {
		w = cam.pixelWidth;
		h = cam.pixelHeight;
		
		// create the texture if it hasn't been created yet or the window has been rescaled
		if (tex == null || tex.width != w || tex.height != h) {
			if (tex != null) {
				tex.Release();
				tex2.Release();
			}

			tex = new RenderTexture(w, h, 32);
			tex.enableRandomWrite = true;
			tex.Create();
			
			tex2 = new RenderTexture(w, h, 32);
			tex2.format = RenderTextureFormat.ARGBFloat;
			tex2.Create();
			ReRender();
		}
	}

	// take a screenshot without the ui
	public IEnumerator TakeScreenshot(bool ui) {
		
		// prepare
		yield return null;
		if (!ui) canvas.SetActive(false);
		ReRender();
 
		// take
		yield return new WaitForEndOfFrame();
		String path = Application.persistentDataPath + "/screenshots/" + shader.name + "/" + w + "x" + h + "/";
		Directory.CreateDirectory(path);
		string n = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
		ScreenCapture.CaptureScreenshot(path + n);
		Debug.Log("Screenshot saved as " + n);
		yield return null;
 
		// reset
		if (!ui) canvas.SetActive(true);
		ReRender();
	}

	// option setter
	public void SetOption(string n, object value) {
		GetType().GetProperty(n)?.SetValue(this, value);
		ReRender();
	}

	// option getter
	public object GetOption(string n) {
		return GetType().GetProperty(n)?.GetValue(this);
	}
	
	/*
	 * Utils
	 */

	// convert spherical coordinates to a point on a unit sphere
	public Vector3 SphericalCoordsToCartesianCoords(float azimuth, float elevation) {
		float x = Mathf.Cos(ToRadians(azimuth)) * Mathf.Cos(ToRadians(elevation));
		float y = Mathf.Sin(ToRadians(elevation));
		float z = Mathf.Sin(ToRadians(azimuth)) * Mathf.Cos(ToRadians(elevation));
		return new Vector3(x, y, z).normalized; // just in case

		// convert to radians
		float ToRadians(float angle) {
			return angle * Mathf.PI / 180;
		}
	}

	// convert a point on a unit sphere to spherical coordinates
	public Vector2 CartesianCoordsToSphericalCoords(Vector3 c) {
		c.Normalize(); // just in case
		float elevation = ToDegree(Mathf.Atan(c.y / Mathf.Sqrt(c.x * c.x + c.z * c.z)));
		float azimuth = 90 - ToDegree(Mathf.Atan(c.x / c.z));
		if (c.z < 0) azimuth += 180;
		return new Vector2(azimuth, elevation);
		
		// convert to degrees
		float ToDegree(float angle) {
			float a = angle / Mathf.PI * 180;
			return a;
		}
	}

	public enum CameraType {
		None, Orbit, Free
	}
}