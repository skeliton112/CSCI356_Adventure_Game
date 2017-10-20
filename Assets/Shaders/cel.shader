Shader "Custom/Cel"
{
    Properties
    {
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}

        _LitColour ("Lit Colour", Color) = (0.9, 0.9, 0.9, 1)
        _UnlitColour ("Unlit Colour", Color) = (0.6, 0.6, 0.6, 1)
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

            struct v2f
            {
                SHADOW_COORDS(1)
                fixed3 diff : COLOR0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;

                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 _LitColour;
			fixed4 _UnlitColour;
			fixed  _Cutoff;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow;
                fixed4 col;

                if (dot (lighting, lighting) > _Cutoff * _Cutoff)
                	col = _LitColour;
                else
                	col = _UnlitColour;

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

            struct v2f
            {
                SHADOW_COORDS(1)
                float4 pos : SV_POSITION;
                fixed3 worldPos : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);

                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 _LitColour;
			fixed  _Cutoff;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lightPos = _WorldSpaceLightPos0.xyz - i.worldPos;
                fixed3 lighting = _LightColor0.rgb * shadow / (1 + dot(lightPos, lightPos));

                if (dot (lighting, lighting) <= _Cutoff * _Cutoff)
                	discard;

                return _LitColour;
            }
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}