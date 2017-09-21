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
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal,1));

                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 _LitColour;
			fixed4 _UnlitColour;
			fixed  _Cutoff;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow + i.ambient;
                fixed4 col;

                if (dot (lighting, lighting) > _Cutoff * _Cutoff)
                	col = _LitColour;
                else
                	col = _UnlitColour;

                return col;
            }
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}