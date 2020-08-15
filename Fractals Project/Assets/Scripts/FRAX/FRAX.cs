
using UnityEngine;
using Utils;

public class FRAX : App {

	public Material depth;
	
	private AnimationController anim;
	private float edge = 1.8f;
	private Vector3 curveSpace = Vector3.zero;
	private float curve = 1;
	
	private void Start() {
		cam.depthTextureMode = DepthTextureMode.Depth;
		controls.minRadius = 2;
		controls.maxRadius = 2;
		
		anim = gameObject.AddComponent<AnimationController>();
		anim.RegisterAnimation(0, edge, 0.4f, 1.8f, 2.45f);
		anim.RegisterAnimation(1, curveSpace.x, 0.3f, -1, 1);
		anim.RegisterAnimation(2, curveSpace.y, 0.2f, -1, 1);
		anim.RegisterAnimation(3, curveSpace.z, 0.1f, -1, 1);
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
		shader.SetFloat("Edge", edge);
		shader.SetVector("CurveSpace", curveSpace * curve);
		
		shader.Dispatch(0, Mathf.CeilToInt(w / 8f), Mathf.CeilToInt(h / 8f), 1);
	}

	private new void Update() {
		base.Update();

		// update animations
		edge = anim.GetAnimation(0);
		curveSpace = new Vector3(anim.GetAnimation(1), anim.GetAnimation(2), anim.GetAnimation(3));
		
		ReRender();
	}
	
	/*
	 * Options
	 */
	
	public float O_Curve {
		get => curve;
		set => curve = value;
	}
}
