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
}