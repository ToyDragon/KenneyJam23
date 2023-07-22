Shader "Universal Render Pipeline/Custom/Shine"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _ColorB ("ColorB", Color) = (.5, .5, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            float4 _ColorB;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float3 lerp(float3 a, float3 b, float t) {
                return a + (b-a) * t;
            }

            v2f vert (appdata v)
            {
                v2f o;
                float3 cameraRight = UNITY_MATRIX_V[0].xyz;
                float3 cameraUp = UNITY_MATRIX_V[1].xyz;
                v.vertex = float4(v.vertex.y * cameraUp + v.vertex.x * cameraRight, 1);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float timeT = _SinTime.z*.5 + .5;
                float dist = 1 - min(max(length(i.uv - float2(.5, .5))*2 + timeT*.7, 0), 1);
                return float4(lerp(_Color.rgb, _ColorB.rgb, timeT), dist);
            }
            ENDCG
        }
    }
}
