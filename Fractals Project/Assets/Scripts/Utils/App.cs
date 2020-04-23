using System;
using System.Numerics;
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

	// below are just camera/player controls

	protected float sens = 10f;
	protected CameraType cameraType = CameraType.None;

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
		c.Default.ToggleCam.canceled += ctx => { switchCursor(); };
		c.Default.Back.canceled += ctx => { GetComponent<SceneLoader>().Load(0); };
		c.Default.ToggleOptions.canceled += ctx => { options.SetActive(!options.activeSelf); };
	}

	private void OnEnable() {
		cam = GetComponent<Camera>();
		c.Enable();
	}
	private void OnDisable() { c.Disable(); }

	private void switchCursor() {
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
	
	void Update () {
		if (cameraType != CameraType.None) {
			if (Cursor.lockState == CursorLockMode.Locked) {
				switch (cameraType) {
					
					// free view camera controls
					case CameraType.Free:
						ReRender();
						transform.Rotate(-cursor.y * Time.smoothDeltaTime * sens, cursor.x * Time.smoothDeltaTime * sens, -tilt * Time.deltaTime * sens);
						transform.Translate(new Vector3(move.x * Time.deltaTime * sens, 0, move.y * Time.deltaTime * sens));
						break;
					
					// orbit camera controls
					case CameraType.Orbit:
						ReRender();
						break;
					
				}
			}
		}
	}

	protected enum CameraType {
		None, Orbit, Free
	}
}