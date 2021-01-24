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
	public CameraType cameraType = CameraType.None;
	public AnimationController ac { get; private set; }
	public OfflineRenderer or { get; private set; }
	public OptionsManager om { get; private set; }
	public ControlsHelper controls { get; private set; }
	private bool update;
	private TextMeshProUGUI fUGUI;

	// awake
	protected void Awake() {
		// important components
		controls = gameObject.AddComponent<ControlsHelper>();
		ac = gameObject.AddComponent<AnimationController>();
		or = gameObject.AddComponent<OfflineRenderer>();
		om = gameObject.AddComponent<OptionsManager>();

		// Load the _Loading scene if necessary
		if (SceneManager.sceneCount == 1)
			SceneManager.LoadSceneAsync((int) SceneLoader.SceneNames.Loading, LoadSceneMode.Additive);

		// register the controls
		controls.RegisterControls();

		// back button
		canvas.transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick
			.AddListener(() => SceneLoader.LoadByIndex((int) SceneLoader.SceneNames.MainMenu));
	}

	// enable
	protected void OnEnable() {
		cam = GetComponent<Camera>();
		fUGUI = canvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		InitRenderTexture();
	}

	// update
	protected void Update() {
		fUGUI.text = Mathf.RoundToInt(1.0f / RequestSmoothDeltaTime()) + " FPS";
	}

	/*
	 * Rendering
	 */

	// rerender
	public void ReRender() {
		update = true;
	}

	// render function
	protected abstract void Render(RenderTexture s);

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

	// deltaTime
	public float RequestDeltaTime() {
		return or.RequestDeltaTime(false);
	}

	public float RequestSmoothDeltaTime() {
		return or.RequestDeltaTime(true);
	}

	public enum CameraType {
		None,
		Orbit,
		Free
	}
}