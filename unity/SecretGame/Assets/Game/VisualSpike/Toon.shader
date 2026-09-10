Shader "SecretGame/VisualSpike/Toon"
{
    Properties
    {
        _BaseColor("Palette", Color) = (1,1,1,1)
        _ShadowColor("Cool shadow tint", Color) = (.31,.4,.55,1)
        _EmissionColor("Indicator", Color) = (0,0,0,1)
        _BandLow("Shadow threshold", Range(0,1)) = .28
        _BandHigh("Light threshold", Range(0,1)) = .67
        _BaseMap("Base", 2D) = "white" {}
        _Cutoff("Cutoff", Float) = .5
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
        Pass
        {
            Name "ToonForward"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor, _ShadowColor, _EmissionColor, _BaseMap_ST;
            float _BandLow, _BandHigh, _Cutoff;
            CBUFFER_END
            struct A { float4 positionOS:POSITION; float3 normalOS:NORMAL; };
            struct V { float4 positionCS:SV_POSITION; float3 positionWS:TEXCOORD0; float3 normalWS:TEXCOORD1; };
            V Vert(A input)
            {
                V output;
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }
            half4 Frag(V input):SV_Target
            {
                Light key = GetMainLight(TransformWorldToShadowCoord(input.positionWS));
                float value = saturate(dot(normalize(input.normalWS), key.direction)) * key.shadowAttenuation;
                float aa = max(fwidth(value), .008);
                float mid = smoothstep(_BandLow-aa, _BandLow+aa,value);
                float high = smoothstep(_BandHigh-aa, _BandHigh+aa,value);
                float3 band = lerp(_ShadowColor.rgb, float3(.73,.77,.83), mid);
                band = lerp(band, float3(1,1,.96),high);
                float3 lit = _BaseColor.rgb * band * max(key.color, float3(.65,.65,.65));
                return half4(lit + _EmissionColor.rgb,1);
            }
            ENDHLSL
        }
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
        UsePass "Universal Render Pipeline/Lit/DepthOnly"
    }
}
