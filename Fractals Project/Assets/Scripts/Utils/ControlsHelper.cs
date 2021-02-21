using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsHelper : MonoBehaviour {
	private App app;
	private Controls controls;

	// options
	public float sensitivity = 10, minRadius = 1, maxRadius = 5;

	// default
	[HideInInspector] public Vector2 move, cursor, dragPosition;
	[HideInInspector] public float tilt, deltaZoom;
	[HideInInspector] public bool dragging, rightClick;

	// flight
	[HideInInspector] public float throttle, roll, yaw, pitch;

	// Camera Animation
	[Header("Animation")]
	[Range(10, 60)] public int FPS = 30;
	public bool animateCameraElevation;
	public bool animateCameraAzimuth;
	public bool animateCameraRadius;
	private int animElevationID, animAzimuthID, animRadiusID;

	private void OnEnable() {
		app = GetComponent<App>();
	}

	private void Start() {
		controls.Enable();
	}

	private void OnDisable() {
		controls.Disable();
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
		controls.Default.LetfClick.canceled += ctx => EndDrag();
		controls.Default.RightClick.performed += ctx => rightClick = true;
		controls.Default.RightClick.canceled += ctx => rightClick = false;
		controls.Default.Back.canceled += ctx => { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; SceneLoader.LoadByIndex((int) SceneLoader.SceneNames.MainMenu); };
		controls.Default.ToggleOptions.canceled += ctx => options.SetActive(!options.activeSelf);
		controls.Default.Zoom.performed += ctx => deltaZoom = ctx.ReadValue<float>();
		controls.Default.Zoom.canceled += ctx => deltaZoom = 0f;
		controls.Default.Screenshot.canceled += ctx => app.StartCoroutine(app.TakeScreenshot(Input.GetKey(KeyCode.LeftShift)));
		controls.Default.Reload.canceled += ctx => SceneLoader.Reload();
		controls.Default.Record.canceled += ctx => app.or.ToggleRendering(30, Input.GetKey(KeyCode.LeftShift));

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
	private void Update() {
		Transform t = app.transform;
		bool c = Cursor.lockState == CursorLockMode.Locked;
		switch (app.cameraType) {
			case App.CameraType.Free:
				if (c && !app.or.isRendering)
					UpdateFreeCamera(t);
				break;
			case App.CameraType.Orbit:
				UpdateOrbitCamera(t, c);
				break;
		}
	}

	private void UpdateFreeCamera(Transform t) {
		t.Rotate(new Vector3(
			-cursor.y * app.RequestSmoothDeltaTime(),
			cursor.x * app.RequestSmoothDeltaTime(),
			-tilt * app.RequestSmoothDeltaTime() * 10) * 5);
		t.Translate(new Vector3(
			move.x * app.RequestSmoothDeltaTime() * sensitivity,
			0,
			move.y * app.RequestSmoothDeltaTime() * sensitivity));
		app.ReRender();
	}

	private void UpdateOrbitCamera(Transform t, bool c) {
		Vector3 pos = t.localPosition;
		float r = pos.magnitude;
		Vector2 angles = app.CartesianCoordsToSphericalCoords(pos.normalized);

		// register
		if (!animateCameraAzimuth) animAzimuthID = 0;
		else if (animAzimuthID == 0) animAzimuthID = app.ac.Register(angles.x, 15, 0, 360, false);
		if (!animateCameraElevation) animElevationID = 0;
		else if (animElevationID == 0) animElevationID = app.ac.Register(angles.y, 0.05f, -80, 80, true);
		if (!animateCameraRadius) animRadiusID = 0;
		else if (animRadiusID == 0) animRadiusID = app.ac.Register(r, 0.06f, minRadius, maxRadius, true);

		// get
		if (animateCameraAzimuth) angles.x = app.ac.Get(animAzimuthID);
		else if (!app.or.isRendering && c) angles.x -= cursor.x * app.RequestSmoothDeltaTime() * sensitivity;
		if (animateCameraElevation) angles.y = app.ac.Get(animElevationID);
		else if (!app.or.isRendering && c) angles.y -= cursor.y * app.RequestSmoothDeltaTime() * sensitivity;
		if (animateCameraRadius) r = app.ac.Get(animRadiusID);
		else if (!app.or.isRendering && c) r -= deltaZoom * sensitivity / 3000;

		if (c || animateCameraAzimuth || animateCameraElevation || animateCameraRadius) {
			ClampOrbitVars(ref r, ref angles);
			t.localPosition = app.SphericalCoordsToCartesianCoords(angles.x, angles.y) * r;
			Transform p = t.parent;
			t.LookAt(p ? p.position : Vector3.zero);
			app.ReRender();
		}
	}

	// clamp variables for orbit controls (radius and angles)
	private void ClampOrbitVars(ref float r, ref Vector2 angles) {
		r = Mathf.Clamp(r, minRadius, maxRadius);
		if (angles.x < 0) angles.x += 360;
		if (angles.x >= 360) angles.x -= 360;
		angles.y = Mathf.Clamp(angles.y, -80, 80);
	}

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