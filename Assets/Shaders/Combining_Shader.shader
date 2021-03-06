﻿Shader "Post Processing/Combining Shader"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}

		[HideInInspector]_FadeAmount ("Amount", Range(0, 1)) = 0
		[HideInInspector]_CircleCentre ("Centre", Vector) = (0.75, 0.25, 1, 1)
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

			fixed4 _CircleCentre;
			fixed  _FadeAmount;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);

				fixed x = _CircleCentre.w * (i.uv.x - _CircleCentre);

				if (x*x + (i.uv.y - _CircleCentre.y)*(i.uv.y - _CircleCentre.y) < _FadeAmount * _CircleCentre.z || _FadeAmount == 1)
					col = tex2D(_MainTex, i.uv);

				return col;
			}
			ENDCG
		}
	}
}
