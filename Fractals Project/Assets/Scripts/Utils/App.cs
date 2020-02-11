using System;
using UnityEngine;

public class App : MonoBehaviour {
	
	public ComputeShader shader;
	protected RenderTexture tex;
	protected Camera cam;
	protected int w;
	protected int h;

	protected void Init() {
		cam = Camera.current;
		w = cam.pixelWidth;
		h = cam.pixelHeight;
		
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