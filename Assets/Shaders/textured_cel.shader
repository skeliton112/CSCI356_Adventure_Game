Shader "Custom/Textured Cel"
{
    Properties
    {
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}

        _LitTexture ("Texture", 2D) = "white" {}
        _Shade ("Shade", Color) = (0.6, 0.6, 0.6, 1)
        _Cutoff ("Cutoff", Range (0, 5)) = 1
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "AutoLight.cginc"

            struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

            struct v2f
            {
                SHADOW_COORDS(1)
                fixed3 diff : COLOR0;
				float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _LitTexture;
            float4 _LitTexture_ST;
            fixed4 _Shade;
			fixed  _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
				o.uv = TRANSFORM_TEX (v.uv, _LitTexture);

                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow;
                fixed4 col = tex2D(_LitTexture, i.uv);;

                if (dot (lighting, lighting) <= _Cutoff * _Cutoff)
                	col *= _Shade;

                return col;
            }
            ENDCG
        }

        Pass
        {
            Tags {"LightMode"="ForwardAdd"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #pragma multi_compile_fwdadd_fullshadows nolightmap nodirlightmap nodynlightmap novertexlight

            #include "AutoLight.cginc"

            struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

            struct v2f
            {
                SHADOW_COORDS(1)
                float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
                fixed3 worldPos : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				o.uv = v.uv;

                TRANSFER_SHADOW(o)
                return o;
            }

            sampler2D _LitTexture;
			fixed  _Cutoff;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lightPos = _WorldSpaceLightPos0.xyz - i.worldPos;
                fixed3 lighting = _LightColor0.rgb * shadow / (1 + dot(lightPos, lightPos));

                if (dot (lighting, lighting) <= _Cutoff * _Cutoff)
                	discard;

                return tex2D(_LitTexture, i.uv);
            }
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}