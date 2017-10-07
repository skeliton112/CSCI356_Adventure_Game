Shader "Custom/Water"
{
	Properties
	{
		_MainTex ("Mask", 2D) = "white" {}
		_Water ("Water Texture", 2D) = "white" {}

		_Light ("Light", Color) = (0,1,1,1)
		_Dark ("Dark", Color) = (0,0,1,1)
		_Fade ("Dark fade", range(0, 10)) = 5
		_Foam ("Foam", Color) = (1,1,1,1)

		_Flow ("Flow", Vector) = (0,0,0,0)
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
				float2 mask_uv : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _Water;
			float4 _Water_ST;

			fixed4 _Light;
			fixed4 _Dark;
			fixed4 _Foam;
			fixed4 _Flow;
			fixed  _Fade;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Water);
				o.mask_uv = v.uv;

				return o;
			}

			fixed2 complex_sine (fixed2 uv){
				return fixed2 (sin (3 * uv.x + 2 * _Flow.z * _Time.x) + sin (2.45 * uv.y + _Flow.z * 1.3 * _Time.x) + _Flow.z * (_SinTime.x + _SinTime.w),
								sin (2 * uv.y + _Flow.w * 3 * _Time.x) + sin (4.23 * uv.x + _Flow.w * 2.2 * _Time.x) + _Flow.w * (_SinTime.z + _SinTime.y)) / 10;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;

				if (tex2D(_MainTex, i.mask_uv).a < 0.5) discard;

				fixed val = tex2D(_Water, 1.3 * i.uv + complex_sine (i.uv) + _Flow.xy * _Time.x).x;
				if (val > 0.5)
					col = _Foam;
				else
					col = _Light;

				val = tex2D(_Water, i.uv + _Flow.xy * _Time.x + fixed2(0.75, 0.5) + complex_sine (i.uv + fixed2(0.25, 0.25))).x;
				if (val > 0.5)
					col = (_Fade * col + _Dark) / (1 + _Fade);

				return col;
			}
			ENDCG
		}
	}
}
