using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour {
	
	private App app;
	private Dictionary<string, int> animatedOptions1 = new Dictionary<string, int>();
	private Dictionary<string, Int2> animatedOptions2 = new Dictionary<string, Int2>();
	private Dictionary<string, Int3> animatedOptions3 = new Dictionary<string, Int3>();

	private void OnEnable() {
		app = GetComponent<App>();
	}

	private void Update() {
		foreach (var v in animatedOptions1) {
			SetOption(v.Key, app.ac.Get(v.Value));
		}

		foreach (var v in animatedOptions2) {
			SetOption(v.Key, new Vector2(
				app.ac.Get(v.Value.x),
				app.ac.Get(v.Value.y)));
		}

		foreach (var v in animatedOptions3) {
			SetOption(v.Key, new Vector3(
				app.ac.Get(v.Value.x),
				app.ac.Get(v.Value.y),
				app.ac.Get(v.Value.z)));
		}
	}

	public void SetOption(string n, object value) {
		app.GetType().GetProperty(n)?.SetValue(app, value);
		app.ReRender();
	}

	// option getter
	public object GetOption(string n) {
		return app.GetType().GetProperty(n)?.GetValue(app);
	}

	public void StartOptionAnimation(string n, float speed, float min, float max, bool oscillating) {
		object option = GetOption(n);
		if (!(option is int) && !(option is float)) return;
		float value = (float) option;
		animatedOptions1.Add(n, app.ac.Register(value, speed, min, max, oscillating));
	}

	public void StartOptionAnimation(string n, Vector2 speed, Vector2 min, Vector2 max, bool oscillating) {
		object option = GetOption(n);
		if (!(option is Vector2)) return;
		Vector2 value = (Vector2) option;
		animatedOptions2.Add(n, new Int2(
			app.ac.Register(value.x, speed.x, min.x, max.x, oscillating),
			app.ac.Register(value.y, speed.y, min.y, max.y, oscillating)
		));
	}

	public void StartOptionAnimation(string n, Vector3 speed, Vector3 min, Vector3 max, bool oscillating) {
		object option = GetOption(n);
		if (!(option is Vector3)) return;
		Vector3 value = (Vector3) option;
		animatedOptions3.Add(n, new Int3(
			app.ac.Register(value.x, speed.x, min.x, max.x, oscillating),
			app.ac.Register(value.y, speed.y, min.y, max.y, oscillating),
			app.ac.Register(value.z, speed.z, min.z, max.z, oscillating)
		));
	}

	public void StopOptionAnimation(string n) {
		if (animatedOptions1.ContainsKey(n)) {
			app.ac.Unregister(animatedOptions1[n]);
			animatedOptions1.Remove(n);
		} else if (animatedOptions2.ContainsKey(n)) {
			app.ac.Unregister(animatedOptions2[n].x);
			app.ac.Unregister(animatedOptions2[n].y);
			animatedOptions2.Remove(n);
		} else if (animatedOptions3.ContainsKey(n)) {
			app.ac.Unregister(animatedOptions3[n].x);
			app.ac.Unregister(animatedOptions3[n].y);
			app.ac.Unregister(animatedOptions3[n].z);
			animatedOptions3.Remove(n);
		}
	}

	struct Int2 {
		public int x;
		public int y;

		public Int2(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	struct Int3 {
		public int x;
		public int y;
		public int z;

		public Int3(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
}