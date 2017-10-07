Shader "Custom/WorldSpaceNormals"
{
	Properties
	{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
	}
    SubShader
    {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // include file that contains UnityObjectToWorldNormal helper function
            #include "UnityCG.cginc"

            struct v2f {
                // we'll output world space normal as one of regular ("texcoord") interpolators
                float4 worldNormal : TEXCOORD0;
				float2 uv : TEXCOORD1;
                float4 pos : SV_POSITION;
            };

            // vertex shader: takes object space normal as input too
            v2f vert (float4 vertex : POSITION, float3 normal : NORMAL, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                fixed3 t = UnityObjectToViewPos (vertex);
                o.worldNormal.w = -t.z / 10; 
                // UnityCG.cginc file contains function to transform
                // normal from object to world space, use that
                o.worldNormal.xyz = UnityObjectToWorldNormal(normal);
                o.uv = uv;
                return o;
            }
			
			sampler2D _MainTex;
            
            fixed4 frag (v2f i) : SV_Target
            {
            	if (tex2D(_MainTex, i.uv).a < 0.5)
            		discard;
            	fixed3 n = i.worldNormal*0.5 + 0.5;
                fixed4 c = fixed4 (n.x, n.y, n.z, 1);
                return c;
            }
            ENDCG
        }
    }
}