Shader "SecretGame/VisualSpike/Outline"
{
    Properties { _Color("Ink", Color) = (.035,.045,.075,1) _Width("Pixels", Float) = 1.35 }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry+1" }
        Pass
        {
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front
            ZWrite On
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            CBUFFER_START(UnityPerMaterial)
            float4 _Color; float _Width;
            CBUFFER_END
            struct A { float4 vertex:POSITION; float3 normal:NORMAL; };
            float4 Vert(A input):SV_POSITION
            {
                float4 clip = TransformObjectToHClip(input.vertex.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normal);
                float2 projected = mul((float3x3)UNITY_MATRIX_V,normalWS).xy;
                float lengthSquared = dot(projected,projected);
                projected *= rsqrt(max(lengthSquared,1e-6));
                clip.xy += projected * (2 * _Width / _ScreenParams.xy) * clip.w;
                return clip;
            }
            half4 Frag():SV_Target { return _Color; }
            ENDHLSL
        }
    }
}
