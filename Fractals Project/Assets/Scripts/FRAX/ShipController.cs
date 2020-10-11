using UnityEngine;
using UnityEngine.VFX;

public class ShipController : MonoBehaviour {

	public VisualEffect engine1;
	public VisualEffect engine2;
	
	// (velocity, acceleration, "air resistance")
	private Vector3 position = new Vector3(0, 0, 1);
	private Vector3 roll = new Vector3(0, 0, 5); // delta roll
	private Vector3 yaw = new Vector3(0, 0, 2); // delta yaw
	private Vector3 pitch = new Vector3(0, 0, 2); // delta pitch
	
	private Transform t;
	private FRAX app;
	private Vector3 cam;
	private ControlsHelper c;

	private void Start() {
		t = GetComponent<Transform>();
		app = t.GetChild(0).GetComponent<FRAX>();
		cam = GetCameraCoords(app.transform.localPosition);
		c = app.controls;
	}

	private void Update() {
		
		// update values
		position[1] = c.throttle * 9;
		UpdateValue(ref position, -3, 20);
		roll[1] = c.roll * 7;
		UpdateValue(ref roll, -5, 5);
		yaw[1] = c.yaw * 3;
		UpdateValue(ref yaw, -2, 2);
		pitch[1] = c.pitch * 3;
		UpdateValue(ref pitch, -2, 2);
		
		
		// update ship
		t.position += t.forward * (position.x * Time.deltaTime);
		t.Rotate(new Vector3(pitch[0], yaw[0], roll[0]) * (Time.deltaTime * Mathf.PI * 20));
		UpdateEngines();
		
		// update camera
		app.transform.localPosition = app.SphericalCoordsToCartesianCoords(cam.x - yaw[0] * 3, cam.y - pitch[0] * 3) * (cam.z + position[0] / 20);
		app.transform.localRotation = Quaternion.Euler(0, 0 , -roll[0] * 3);

	}

	private void UpdateValue(ref Vector3 v, float min, float max) { // min and max velocity
		v[0] += v[1] * Time.deltaTime; // velocity += acceleration
		if (v[0] < 0) { // |velocity| -= "air resistance"
			if (v[1] == 0) v[0] += v[2] * Time.deltaTime;
			v[0] = Mathf.Clamp(v[0], min, 0);
		} else if (v[0] > 0) {
			if (v[1] == 0) v[0] -= v[2] * Time.deltaTime;
			v[0] = Mathf.Clamp(v[0], 0, max);
		}
	}

	private void UpdateEngines() {
		float v = Mathf.Abs(position[0] / 20);
		float a = position[1] / 10;
		engine1.SetFloat("Power", 0.1f + Mathf.Max(Mathf.Abs(a), 0.5f) * v);
		engine2.SetFloat("Power", 0.1f + Mathf.Max(Mathf.Abs(a), 0.5f) * v);
		engine1.SetFloat("Irregularity", 10 - a * 10  * v);
		engine2.SetFloat("Irregularity", 10 - a * 10  * v);
		
		Vector2 directionL = new Vector2(-pitch[0] + roll[0] / 3, -yaw[0]) / 3;
		Vector2 directionR = new Vector2(-pitch[0] - roll[0] / 3, -yaw[0]) / 3;
		engine1.SetVector2("Direction", directionL);
		engine2.SetVector2("Direction", directionR);
	}
	
	private Vector3 GetCameraCoords(Vector3 pos) {
		Vector2 angles = app.CartesianCoordsToSphericalCoords(pos.normalized);
		return new Vector3(angles.x, angles.y, pos.magnitude);
	}
}
