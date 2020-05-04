using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public abstract class App : MonoBehaviour {
	
	// variables
	public ComputeShader shader;
	public GameObject options;
	protected RenderTexture tex;
	protected Camera cam;
	protected int w;
	protected int h;

	// only update when needed
	private bool update = false;
	protected void ReRender() { update = true; }
	
	protected abstract void Render(RenderTexture s);
	
	// render update method
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture s, RenderTexture d) {
		Init();
		if (update) {
			Render(s);
			update = false;
		}
		Graphics.Blit(tex, d);
	}

	// init function
	private void Init() {
		w = cam.pixelWidth;
		h = cam.pixelHeight;
		
		// create the texture only if it hasn't been created yet or the window has been rescaled
		// from https://github.com/SebLague
		if (tex == null || tex.width != w || tex.height != h) {
			if (tex != null) {
				tex.Release();
			}

			tex = new RenderTexture(w, h, 32);
			tex.enableRandomWrite = true;
			tex.Create();
			ReRender(); // rerender needed
		}
	}

	// from https://answers.unity.com/questions/850451/capturescreenshot-without-ui.html and modified
	private IEnumerator TakeScreenshot() {

		// prepare
		yield return null;
		options.transform.parent.gameObject.SetActive(false);
		ReRender();
 
		// take
		yield return new WaitForEndOfFrame();
		String path = Application.persistentDataPath + "/screenshots/" + shader.name + "/";
		String name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
		ScreenCapture.CaptureScreenshot(path + name);
		Debug.Log("Screenshot saved as " + name);
		yield return null;
 
		// reset
		options.transform.parent.gameObject.SetActive(true);
		ReRender();
	}

	// option stuff
	
	public void SetOption(string n, object value) {
		try {
			GetType().GetProperty(n).SetValue(this, value);
		} catch (Exception e) {
			Debug.LogError("Invalid option name");
			throw e;
		}
	}

	public object GetOption(string n) {
		try {
			ReRender();
			return GetType().GetProperty(n).GetValue(this);
		} catch (Exception e) {
			Debug.LogError("Invalid option name");
			throw e;
		}
	}

	// below are just camera/player controls

	protected float sens = 10f;
	protected CameraType cameraType = CameraType.None;
	
	// input variables
	private Controls c;
	private Vector2 move;
	private Vector2 cursor;
	private float tilt;
	protected float deltaZoom;
	protected bool drag;
	protected Vector2 dp;

	// register all controls
	private void Awake() {
		c = new Controls();
		c.Default.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
		c.Default.Move.canceled += ctx => move = Vector2.zero;
		c.Default.Look.performed += ctx => cursor = ctx.ReadValue<Vector2>();
		c.Default.Look.canceled += ctx => cursor = Vector2.zero;
		c.Default.Tilt.performed += ctx => tilt = ctx.ReadValue<float>();
		c.Default.Tilt.canceled += ctx => tilt = 0;
		c.Default.ScreenClick.performed += ctx => { StartDrag(); };
		c.Default.ScreenClick.canceled += ctx => { EndDrag(); SwitchCursor(); };
		c.Default.Back.canceled += ctx => { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; GetComponent<SceneLoader>().Load(0); };
		c.Default.ToggleOptions.canceled += ctx => { options.SetActive(!options.activeSelf); };
		c.Default.Zoom.performed += ctx => deltaZoom = ctx.ReadValue<float>();
		c.Default.Zoom.canceled += ctx => deltaZoom = 0f;
		c.Default.Screenshot.canceled += ctx => { StartCoroutine(TakeScreenshot()); };
	}

	// enable controls
	private void OnEnable() {
		Directory.CreateDirectory(Application.persistentDataPath + "/screenshots/" + shader.name + "/");
		cam = GetComponent<Camera>();
		c.Enable();
	}
	
	// disable controls
	private void OnDisable() { c.Disable(); }

	// switch the cursor
	private void SwitchCursor() {
		if (cameraType != CameraType.None) {
			if (Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			} else if (!EventSystem.current.IsPointerOverGameObject()) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}

	// drag start
	private void StartDrag() {
		if (!EventSystem.current.IsPointerOverGameObject()) {
			drag = true;
			dp = Input.mousePosition;
		}
	}

	// drag end
	private void EndDrag() {
		drag = false;
	}
	
	// update
	void Update () {
		if (cameraType != CameraType.None) {
			if (Cursor.lockState == CursorLockMode.Locked) {
				switch (cameraType) {
					
					// free view camera controls
					case CameraType.Free:
						transform.Rotate(-cursor.y * Time.smoothDeltaTime * 5, cursor.x * Time.smoothDeltaTime * 5, -tilt * Time.deltaTime * 5);
						transform.Translate(new Vector3(move.x * Time.deltaTime * sens, 0, move.y * Time.deltaTime * sens));
						ReRender();
						break;
					
					// orbit camera controls
					case CameraType.Orbit:
						Vector3 pos = transform.position;
						float r = pos.magnitude - deltaZoom * Time.smoothDeltaTime * sens / 30;
						Vector2 angles = CartesianCoordsToSphericalCoords(pos.normalized) + new Vector2(-cursor.x * Time.smoothDeltaTime * sens, -cursor.y * Time.smoothDeltaTime * sens);
						Vector3 vars = ClampOrbitVars(r, angles);
						transform.position = SphericalCoordsToCartesianCoords(vars.x, vars.y) * vars.z;
						transform.LookAt(new Vector3(0,0,0));
						ReRender();
						break;
					
				}
			}
		}

		Vector3 ClampOrbitVars(float r, Vector2 angles) {
			r = Mathf.Clamp(r, 1, 5);
			if (angles.x < 0) angles.x += 360;
			if (angles.x >= 360) angles.x -= 360;
			angles.y = Mathf.Clamp(angles.y, -80, 80);
			return new Vector3(angles.x, angles.y, r);
		}
	}

	// convert spherical coordinates to a point on a unit sphere
	protected Vector3 SphericalCoordsToCartesianCoords(float azimuth, float elevation) {
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
	protected Vector2 CartesianCoordsToSphericalCoords(Vector3 c) {
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
	
	
	
	protected enum CameraType {
		None, Orbit, Free
	}
}