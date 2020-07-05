Shader "Custom/DepthBuffer" {

	Properties {
		_MainTex ("Source", 2D) = "white"
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
	   
		Pass {
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			
			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			float4 frag(v2f i) : COLOR {
				float d = 1 - tex2D(_CameraDepthTexture, i.uv).x;
				return float4(1, 1, 1, 0) * tex2D(_MainTex, i.uv) + float4(0, 0, 0, d);

			}
			
			ENDCG
		}
	}
}