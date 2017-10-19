Shader "Custom/ScrollingMask"
{
	Properties
	{
		_MainTex ("Mask", 2D) = "white" {}
		_Speed ("Speed", range(-20, 20)) = 5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Speed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed2 uv = fixed2 (i.uv.x + _Speed * _Time.x, i.uv.y);
				if (tex2D(_MainTex, uv).x < 0.5)
					discard;

				return fixed4 (1,1,1,1);
			}
			ENDCG
		}
	}
}
