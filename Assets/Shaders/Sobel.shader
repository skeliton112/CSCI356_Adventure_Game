Shader "Post Processing/Sobel"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}

		_Delta ("Delta", Float) = 0.01
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed _Delta;

			float sobel (sampler2D t, float2 uv) {
				float2 d = float2(_Delta, _Delta);
			
				float4 h = float4(0, 0, 0, 0);
				float4 v = float4(0, 0, 0, 0);
				
				h += tex2D(t, (uv + float2(-1.0, -1.0) * d)) *  1.0;
				h += tex2D(t, (uv + float2(-1.0,  0.0) * d)) *  2.0;
				h += tex2D(t, (uv + float2(-1.0,  1.0) * d)) *  1.0;

				h += tex2D(t, (uv + float2( 1.0, -1.0) * d)) * -1.0;
				h += tex2D(t, (uv + float2( 1.0,  0.0) * d)) * -2.0;
				h += tex2D(t, (uv + float2( 1.0,  1.0) * d)) * -1.0;
				
				v += tex2D(t, (uv + float2(-1.0, -1.0) * d)) *  1.0;
				v += tex2D(t, (uv + float2( 0.0, -1.0) * d)) *  2.0;
				v += tex2D(t, (uv + float2( 1.0, -1.0) * d)) *  1.0;

				v += tex2D(t, (uv + float2(-1.0,  1.0) * d)) * -1.0;
				v += tex2D(t, (uv + float2( 0.0,  1.0) * d)) * -2.0;
				v += tex2D(t, (uv + float2( 1.0,  1.0) * d)) * -1.0;


				float4 grad = h * h + v * v;
				float val = sqrt (grad.x) + sqrt (grad.y) + 3 * sqrt (grad.z);
				return val < 0.5 ? 0 : (val < 0.7 ? (val - 0.5) * 5 : 1);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				//return tex2D(_MainTex, i.uv);
				return 1 - sobel(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}
