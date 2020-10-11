using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsHelper {

	private readonly App app;
	private Controls controls;
	
	// options
	public float sensitivity = 10;
	public float minRadius = 1;
	public float maxRadius = 5;
	
	// default
	public Vector2 move;
	public Vector2 cursor;
	public float tilt;
	public float deltaZoom;
	public bool dragging;
	public Vector2 dragPosition;
	public bool rightClick;
	
	// flight
	public float throttle;
	public float roll;
	public float yaw;
	public float pitch;

	public ControlsHelper(App app) {
		this.app = app;
	}
	
	public void RegisterControls() {
		
		// some variables
		GameObject options = app.canvas.transform.GetChild(1).gameObject;
		
		// register default controls
		controls = new Controls();
		controls.Default.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
		controls.Default.Move.canceled += ctx => move = Vector2.zero;
		controls.Default.Look.performed += ctx => cursor = ctx.ReadValue<Vector2>();
		controls.Default.Look.canceled += ctx => cursor = Vector2.zero;
		controls.Default.Tilt.performed += ctx => tilt = ctx.ReadValue<float>();
		controls.Default.Tilt.canceled += ctx => tilt = 0;
		controls.Default.LetfClick.performed += ctx => { StartDrag(); SwitchCursor(); };
		controls.Default.LetfClick.canceled += ctx => { EndDrag(); };
		controls.Default.RightClick.performed += ctx => { rightClick = true; };
		controls.Default.RightClick.canceled += ctx => { rightClick = false; };
		controls.Default.Back.canceled += ctx => { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; SceneLoader.LoadByIndex((int) SceneLoader.SceneNames.MainMenu); };
		controls.Default.ToggleOptions.canceled += ctx => { options.SetActive(!options.activeSelf); };
		controls.Default.Zoom.performed += ctx => deltaZoom = ctx.ReadValue<float>();
		controls.Default.Zoom.canceled += ctx => deltaZoom = 0f;
		controls.Default.Screenshot.canceled += ctx => { app.StartCoroutine(app.TakeScreenshot(Input.GetKey(KeyCode.LeftShift))); };
		
		// register flight controls
		controls.Flight.Throttle.performed += ctx => throttle = ctx.ReadValue<float>();
		controls.Flight.Throttle.canceled += ctx => throttle = 0;
		controls.Flight.Roll.performed += ctx => roll = ctx.ReadValue<float>();
		controls.Flight.Roll.canceled += ctx => roll = 0;
		controls.Flight.Yaw.performed += ctx => yaw = ctx.ReadValue<float>();
		controls.Flight.Yaw.canceled += ctx => yaw = 0;
		controls.Flight.Pitch.performed += ctx => pitch = ctx.ReadValue<float>();
		controls.Flight.Pitch.canceled += ctx => pitch = 0;
	}

	// update
	public void Update() {
		Transform t = app.transform;
		
		if (app.cameraType != App.CameraType.None) {
			if (Cursor.lockState == CursorLockMode.Locked) {
				switch (app.cameraType) {
					
					// free view camera controls
					case App.CameraType.Free:
						t.Rotate(new Vector3(-cursor.y * Time.smoothDeltaTime, cursor.x * Time.smoothDeltaTime, -tilt * Time.deltaTime * 10) * 5);
						t.Translate(new Vector3(move.x * Time.deltaTime * sensitivity, 0, move.y * Time.deltaTime * sensitivity));
						app.ReRender();
						break;
					
					// orbit camera controls
					case App.CameraType.Orbit:
						Vector3 pos = app.transform.localPosition;
						float r = pos.magnitude - deltaZoom * sensitivity / 3000;
						Vector2 angles = app.CartesianCoordsToSphericalCoords(pos.normalized) + new Vector2(-cursor.x * Time.smoothDeltaTime * sensitivity, -cursor.y * Time.smoothDeltaTime * sensitivity);
						Vector3 vars = ClampOrbitVars(r, angles);
						t.localPosition = app.SphericalCoordsToCartesianCoords(vars.x, vars.y) * vars.z;
						t.LookAt(t.parent ? t.parent.position : Vector3.zero);
						app.ReRender();
						break;
					
				}
			}
		}

		// clamp variables for orbit controls (radius and angles)
		Vector3 ClampOrbitVars(float r, Vector2 angles) {
			r = Mathf.Clamp(r, minRadius, maxRadius);
			if (angles.x < 0) angles.x += 360;
			if (angles.x >= 360) angles.x -= 360;
			angles.y = Mathf.Clamp(angles.y, -80, 80);
			return new Vector3(angles.x, angles.y, r);
		}
	}
	
	// enable controls
	public void Enable() { controls.Enable(); }
	
	// disable controls
	public void Disable() { controls.Disable(); }
	
	// toggle cursor visibility
	private void SwitchCursor() {
		if (app.cameraType != App.CameraType.None) {
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
			dragging = true;
			dragPosition = Input.mousePosition;
		}
	}

	// drag end
	private void EndDrag() {
		dragging = false;
	}
}