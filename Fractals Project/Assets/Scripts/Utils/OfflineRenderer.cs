using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class OfflineRenderer : MonoBehaviour {

	private App app;
	private String startTime;
	private int targetFPS;
	private int currentFrame;
	public bool isRendering { get; private set; }

	private void OnEnable() {
		app = GetComponent<App>();
	}

	private void Update() {
		if (isRendering) {
			app.ReRender();
			StartCoroutine(RecordFrame());
		}
	}

	private IEnumerator RecordFrame() {
		
		yield return new WaitForEndOfFrame();
		String path = Application.persistentDataPath + "/recordings/" + app.shader.name + "/" + startTime + " (" + targetFPS + "FPS)/";
		if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		string n = "frame-" + currentFrame + ".png";
		ScreenCapture.CaptureScreenshot(path + n);
		currentFrame++;
		yield return null;
	}

	public float RequestDeltaTime(bool smooth) {
		if (isRendering) return 1f / targetFPS;
		return smooth ? Time.smoothDeltaTime : Time.deltaTime;
	}

	public void StartRendering(int fps, bool ui) {
		if (!isRendering) {
			targetFPS = fps;
			startTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
			currentFrame = 0;
			app.canvas.SetActive(ui);
			isRendering = true;
		}
	}

	public void StopRendering() {
		if (isRendering) {
			isRendering = false;
			app.canvas.SetActive(true);
		}
	}

	public void ToggleRendering(int fps, bool ui) {
		if (isRendering) {
			StopRendering();
		} else {
			StartRendering(fps, ui);
		}
	}
}
