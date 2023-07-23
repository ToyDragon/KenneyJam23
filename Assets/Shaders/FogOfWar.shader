Shader "Custom/PP/FogOfWar" {
    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "FogOfWar"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // The Blit.hlsl file provides the vertex shader (Vert),
            // input structure (Attributes) and output strucutre (Varyings)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            #pragma vertex Vert
            #pragma fragment frag

            TEXTURE2D(_CameraDepthTexture);
            TEXTURE2D(_CameraOpaqueTexture);
            TEXTURE2D(_FogOfWarTexture);
            TEXTURE2D(_RadiationTexture);
            // TEXTURE2D(_NoiseTexture);
            float3 _FeaturePosition;
            float3 _FogColor;

            float LinearEyeDepth(float z)
            {
                return 1.0 / (_ZBufferParams.z * z + _ZBufferParams.w);
            }

            float3 lerp(float3 a, float3 b, float t) {
                return a + (b-a) * t;
            }

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float depth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_PointClamp, input.texcoord).r;
                // world pos calc needs to happen before normalization
                float3 worldPos = ComputeWorldSpacePosition(input.texcoord, depth, UNITY_MATRIX_I_VP);
                float2 fogOfWarUV = worldPos.xz / 1024 + float2(.5, .5);
                float distToFeature = length(worldPos - _FeaturePosition); // _WorldSpaceCameraPos);
                float4 color = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_PointClamp, input.texcoord);

                // float2 noiseUV = worldPos.xz * 0.001 + float2(.5, .5);
                // float noise = SAMPLE_TEXTURE2D_X(_NoiseTexture, sampler_PointClamp, noiseUV);

                float fogOfWarValue = SAMPLE_TEXTURE2D_X(_FogOfWarTexture, sampler_LinearClamp, fogOfWarUV).r;
                float radiationValue = SAMPLE_TEXTURE2D_X(_RadiationTexture, sampler_LinearClamp, fogOfWarUV).r;
                float distanceDimming = 1 - min(max(pow(distToFeature/20, .2), 0), 1);
                color.rb *= (1 - radiationValue);
                
                return float4(lerp(_FogColor, color.rgb, max(distanceDimming, fogOfWarValue)), 1);

                // half3 normal;
                // float depth;
                // DecodeDepthNormal(SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_PointClamp, input.texcoord), depth, normal);
                // return float4(normal, 1);
                // return color * float4(2, 1, 1, 1);
            }
            ENDHLSL

        }
    }
}