
using UnityEngine;

public class FRAX : App {

	public Material depth;
	
	private float edge = 1.8f;
	private Vector3 curveSpace = Vector3.zero;
	private float curve = 1;
	
	// animation ids
	private int edgeID;
	private int curveSpaceXID;
	private int curveSpaceYID;
	private int curveSpaceZID;
	
	private void Start() {
		cam.depthTextureMode = DepthTextureMode.Depth;
		controls.minRadius = 2;
		controls.maxRadius = 2;
		
		// register animations
		edgeID = ac.Register(edge, 0.127f, 1.8f, 2.45f, true);
		curveSpaceXID = ac.Register(curveSpace.x, 0.095f, -1, 1, true);
		curveSpaceYID = ac.Register(curveSpace.y, 0.064f, -1, 1, true);
		curveSpaceZID = ac.Register(curveSpace.z, 0.032f, -1, 1, true);
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
		ac.Get(edgeID, ref edge);
		curveSpace = new Vector3(ac.Get(curveSpaceXID), ac.Get(curveSpaceYID), ac.Get(curveSpaceZID));
		
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
