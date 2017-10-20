Shader "Custom/Smoke"
{
	Properties
	{
		_MainTex ("Mask", 2D) = "white" {}

        _LitColour ("Lit Colour", Color) = (0.9, 0.9, 0.9, 1)
        _UnlitColour ("Unlit Colour", Color) = (0.6, 0.6, 0.6, 1)
        _Cutoff ("Cutoff", Range (0, 5)) = 1

		_XGrad ("X Gradient", Vector) = (0, 0, 10, 1)
		_YGrad ("Y Gradient", Vector) = (0, 0, 10, 1)
		_ZGrad ("Z Gradient", Vector) = (0, 0, 10, 1)
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100

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
				float4 pos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _LitColour;
			fixed4 _UnlitColour;
			fixed  _Cutoff;

			fixed4 _XGrad;
			fixed4 _YGrad;
			fixed4 _ZGrad;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.pos = v.vertex;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 norm = tex2D(_MainTex, i.uv);
				if (norm.a < 0.5) discard;
				norm.a = 0;
				norm.z = -norm.z;

				fixed3 light_dir = fixed3 (max (_XGrad.y, _XGrad.w), max (_YGrad.y, _YGrad.w), -max (_ZGrad.y, _ZGrad.w));
				fixed light = max (dot (norm.xyz, light_dir), 0);

				fixed t = (i.pos.x - _XGrad.x) / (_XGrad.z - _XGrad.x);
				light *= 1 + _XGrad.w * t + _XGrad.y * (1 - t);

				t = (i.pos.z - _ZGrad.x) / (_ZGrad.z - _ZGrad.x);
				light *= 1 + _ZGrad.w * t + _ZGrad.y * (1 - t);

				t = (i.pos.y - _YGrad.x) / (_YGrad.z - _YGrad.x);
				light *= _YGrad.w * t + _YGrad.y * (1 - t);

				if (light > _Cutoff)
 					return _LitColour;
 				else
 					return _UnlitColour;
			}
			ENDCG
		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
