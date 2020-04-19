using System;
using UnityEngine;

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
	
	void Update () {
		if (cameraType != CameraType.None) {
			if (Cursor.lockState == CursorLockMode.Locked) {
				switch (cameraType) {
					
					// free view camera controls
					case CameraType.Free:
						transform.Rotate(0, Input.GetAxis("Mouse X") * sens, 0);
						transform.Rotate(-Input.GetAxis("Mouse Y") * sens, 0, 0);	
						transform.Translate(new Vector3(Input.GetAxis("Horizontal") * sens / 100, 0, Input.GetAxis("Vertical") * sens / 100));
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