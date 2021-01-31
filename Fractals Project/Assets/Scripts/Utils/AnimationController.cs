using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	private App app;
	private Dictionary<int, Params> animations;

	private void OnEnable() {
		app = GetComponent<App>();
		animations = new Dictionary<int, Params>();
	}

	public int Register(float value, float speed, float min, float max, bool osc) {
		value = Mathf.Clamp(value, min, max);
		int id = Guid.NewGuid().GetHashCode();
		if (osc) speed *= Mathf.PI;
		Params p = new Params(value, speed, min, max, osc, value);
		animations.Add(id, MapB(p));
		return id;
	}

	/*public void DecreaseSpeed(int id, float amount) {
		try {
			Params p = animations[id];
			float z = Mathf.Clamp(Mathf.Abs(p.speed) - (app.RequestSmoothDeltaTime() * amount), 0, 100);
			p.speed = z * Mathf.Sign(p.speed);
			animations[id] = p;
		} catch (Exception e) {
			
		}
	}*/

	public void Unregister(int id) {
		animations.Remove(id);
	}

	private void Update() {
		foreach (var v in animations.ToList()) {
			Params p = v.Value;
			p.value += p.speed * app.RequestSmoothDeltaTime();
			animations[v.Key] = Clamp(p);
		}
	}

	public float Get(int id) {
		return MapA(animations[id]).value;
	}
	
	public void Get(int id, ref float value) {
		value =  MapA(animations[id]).value;
	}

	/*public float GetDelta(int id) {
		Params p = MapA(animations[id]);
		float last = p.lastValue;
		p.lastValue = p.value;
		return p.value - last;
	}*/

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
		public float lastValue;

		public Params(float value, float speed, float min, float max, bool osc, float lastValue) {
			this.value = value;
			this.speed = speed;
			this.min = min;
			this.max = max;
			this.osc = osc;
			this.lastValue = lastValue;
		}
	}
}