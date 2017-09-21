Shader "Post Processing/Sobel"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}

		_DeltaX ("delta X", Float) = 0.01
		_DeltaY ("delta Y", Float) = 0.01
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
			fixed _DeltaX;
			fixed _DeltaY;

			float sobel (sampler2D tex, float2 uv) {
				float2 delta = float2(_DeltaX, _DeltaY);
			
				float4 hr = float4(0, 0, 0, 0);
				float4 vt = float4(0, 0, 0, 0);
				
				hr += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
				hr += tex2D(tex, (uv + float2( 0.0, -1.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2( 1.0, -1.0) * delta)) * -1.0;
				hr += tex2D(tex, (uv + float2(-1.0,  0.0) * delta)) *  2.0;
				hr += tex2D(tex, (uv + float2( 0.0,  0.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2( 1.0,  0.0) * delta)) * -2.0;
				hr += tex2D(tex, (uv + float2(-1.0,  1.0) * delta)) *  1.0;
				hr += tex2D(tex, (uv + float2( 0.0,  1.0) * delta)) *  0.0;
				hr += tex2D(tex, (uv + float2( 1.0,  1.0) * delta)) * -1.0;
				
				vt += tex2D(tex, (uv + float2(-1.0, -1.0) * delta)) *  1.0;
				vt += tex2D(tex, (uv + float2( 0.0, -1.0) * delta)) *  2.0;
				vt += tex2D(tex, (uv + float2( 1.0, -1.0) * delta)) *  1.0;
				vt += tex2D(tex, (uv + float2(-1.0,  0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2( 0.0,  0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2( 1.0,  0.0) * delta)) *  0.0;
				vt += tex2D(tex, (uv + float2(-1.0,  1.0) * delta)) * -1.0;
				vt += tex2D(tex, (uv + float2( 0.0,  1.0) * delta)) * -2.0;
				vt += tex2D(tex, (uv + float2( 1.0,  1.0) * delta)) * -1.0;


				float4 grad = hr * hr + vt * vt;
				float val = sqrt (grad.x) + sqrt (grad.y) + 3 * sqrt (grad.z);
				return val < 0.5 ? (val < 0.3 ? 0 : (val - 0.3) * 2.5) : val;
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
