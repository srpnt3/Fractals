using UnityEngine;

public class FRAX : App {

	private Material depth;
	
	private void Start() {
		//cameraType = CameraType.Orbit;
		cam.depthTextureMode = DepthTextureMode.Depth;
		depth = new Material(Shader.Find("Custom/DepthBuffer"));
		controls.minRadius = 2;
		controls.maxRadius = 2;
	}

	protected override void Render(RenderTexture s) {
		
		// write depth buffer into render texture
		Graphics.Blit(s, tex2, depth);
		
		// shader
		shader.SetTexture(0, "Texture", tex);
		shader.SetTexture(0, "Source", tex2);
		shader.SetMatrix("CamToWorld", cam.cameraToWorldMatrix);
		shader.SetMatrix("CamInverseProjection", cam.projectionMatrix.inverse);
		shader.SetFloat("zNear", cam.nearClipPlane);
		shader.SetFloat("zFar", cam.farClipPlane);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
	}

	private new void Update() {
		base.Update();
		ReRender();
	}
}
