using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class App : MonoBehaviour {
	
	// variables
	public ComputeShader shader;
	protected RenderTexture tex;
	protected Camera cam;
	protected int w;
	protected int h;
	
	// init function
	protected void Init() {
		cam = GetComponent<Camera>();
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
		}
	}

	protected float sens = 10f;
	protected CameraType cameraType = CameraType.None;

	// below are just controls

	private Controls c;
	private Vector2 move;
	private Vector2 cursor;
	private float tilt;
	
	private void Awake() {
		c = new Controls();
		c.Default.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
		c.Default.Move.canceled += ctx => move = Vector2.zero;
		c.Default.Cursor.performed += ctx => cursor = ctx.ReadValue<Vector2>();
		c.Default.Cursor.canceled += ctx => cursor = Vector2.zero;
		c.Default.Tilt.performed += ctx => tilt = ctx.ReadValue<float>();
		c.Default.Tilt.canceled += ctx => tilt = 0;
	}

	private void OnEnable() { c.Enable(); }
	private void OnDisable() { c.Disable(); }

	void Update () {
		if (cameraType != CameraType.None) {
			if (Cursor.lockState == CursorLockMode.Locked) {
				switch (cameraType) {
					
					// free view camera controls
					case CameraType.Free:
						transform.Rotate(-cursor.y * Time.smoothDeltaTime * sens, cursor.x * Time.smoothDeltaTime * sens, -tilt * Time.deltaTime * sens);
						transform.Translate(new Vector3(move.x * Time.deltaTime * sens, 0, move.y * Time.deltaTime * sens));
						break;
					
					// orbit camera controls
					case CameraType.Orbit:
						break;
					
				}
				if (Input.GetMouseButtonUp(0)) 
					Cursor.lockState = CursorLockMode.None;
			} else if (Input.GetMouseButtonUp(0)) 
				Cursor.lockState = CursorLockMode.Locked;	
		}
	}

	protected enum CameraType {
		None, Orbit, Free
	}
}