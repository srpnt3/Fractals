using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils {
	public class AnimationController : MonoBehaviour {

		private Dictionary<int, Vector4> animations;

		private void OnEnable() {
			animations = new Dictionary<int, Vector4>();
		}

		public void RegisterAnimation(int id, float value, float speed, float min, float max) {
			animations.Add(id, new Vector4(UnMapValue(value, min, max), speed, min, max));
		}

		private void Update() {
			foreach (var v in animations.ToList()) {
				Vector4 a = v.Value;
				a[0] += a[1] * Time.smoothDeltaTime;
				if (a[0] > 2 * Mathf.PI) a[0] -= 2 * Mathf.PI;
				animations[v.Key] = a;
			}
		}

		public float GetAnimation(int id) {
			Vector4 a = animations[id];
			return MapValue(a[0], a[2], a[3]);
		}

		private float MapValue(float value, float min, float max) {
			float n = (Mathf.Cos(value) + 1) / 2;
			n *= max - min;
			n += min;
			return n;
		}

		private float UnMapValue(float value, float min, float max) {
			float n = value - min;
			n /= max - min;
			n = Mathf.Acos(n * 2 - 1);
			return n;
		}
	}
}