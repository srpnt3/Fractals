using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	private Dictionary<int, Params> animations;

	private void OnEnable() {
		animations = new Dictionary<int, Params>();
	}

	public int Register(float value, float speed, float min, float max, bool osc) {
		int id = GUID.Generate().GetHashCode();
		if (osc) speed *= Mathf.PI;
		Params p = new Params(value, speed, min, max, osc);
		animations.Add(id, MapB(p));
		return id;
	}

	private void Update() {
		foreach (var v in animations.ToList()) {
			Params p = v.Value;
			p.value += p.speed * GetComponent<App>().RequestSmoothDeltaTime();
			animations[v.Key] = Clamp(p);
		}
	}

	public float Request(int id) {
		return MapA(animations[id]).value;
	}
	
	public void Request(int id, ref float value) {
		value =  MapA(animations[id]).value;
	}

	// Map to min;max
	private Params MapA(Params p) {
		if (p.osc) {
			p.value = (Mathf.Cos(p.value) + 1) / 2;
			p.value *= p.max - p.min;
			p.value += p.min;
		}

		return p;
	}

	//Map to -1;1
	private Params MapB(Params p) {
		if (p.osc) {
			p.value -= p.min;
			p.value /= p.max - p.min;
			p.value = Mathf.Acos(p.value * 2 - 1);
		}

		return p;
	}
	
	private Params Clamp(Params p) {
		if (p.osc) {
			if (p.value > 2 * Mathf.PI) p.value -= 2 * Mathf.PI;
			if (p.value < 0) p.value += 2 * Mathf.PI;
		} else {
			float d = p.max - p.min;
			if (p.value > p.max) p.value -= d;
			if (p.value < p.min) p.value += d;
		}

		return p;
	}

	private struct Params {
		public float value;
		public float speed;
		public float min;
		public float max;
		public bool osc;

		public Params(float value, float speed, float min, float max, bool osc) {
			this.value = value;
			this.speed = speed;
			this.min = min;
			this.max = max;
			this.osc = osc;
		}
	}
}