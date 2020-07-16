using UnityEngine;

public class Audio : App {

	private AudioProvider audioProvider;
	private float[] audioArray;
	private float power = 2;
	private int iterations = 8;
	private const int minF = 0;
	private const int maxF = 20000;
	
	private new void OnEnable() {
		base.OnEnable();
		audioArray = new float[64];
		audioProvider = new AudioProvider(64, minF, maxF, a => { audioArray = a; ReRender(); });
		audioProvider.Start();
	}

	private new void OnDisable() {
		base.OnDisable();
		audioProvider.Stop();
	}

	protected override void Render(RenderTexture s) {
		
		// buffer
		ComputeBuffer buffer = new ComputeBuffer(audioArray.Length, sizeof(float));
		buffer.SetData(audioArray);
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", s);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix); 
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		shader.SetFloat("Power", power);
		shader.SetInt("Iterations", iterations);
		shader.SetInt("MinF", minF);
		shader.SetInt("MaxF", maxF);
		shader.SetBuffer(0, "Audio", buffer);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
		
		// dispose buffer
		buffer.Dispose();
	}
	
	/*
	 * options
	 */
	
	public float O_Power {
		get => power;
		set => power = value;
	}
	
	public float O_Iterations {
		get => iterations;
		set => iterations = Mathf.RoundToInt(value);
	}
}
	