Shader "Custom/DistanceFog" {
    Properties {
        _Color ("Object Color", Color) = (0.5, 0.5, 0.5, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0.5, 0.75, 0.75, 1)
        _FogStart ("Fog Start", Float) = 15
        _FogEnd ("Fog End", Float) = 50
    }

    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }
        LOD 100

        Pass {
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _FogColor;
                float _FogStart;
                float _FogEnd;
            CBUFFER_END

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            Varyings vert(Attributes input) {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.worldPos = TransformObjectToWorld(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
                output.worldNormal = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) * _Color;

                Light mainLight = GetMainLight();
                half NdotL = saturate(dot(normalize(input.worldNormal), mainLight.direction));
                half3 diffuse = c.rgb * mainLight.color * NdotL;

                float dist = distance(input.worldPos, _WorldSpaceCameraPos);
                float fogFactor = saturate((_FogEnd - dist) / (_FogEnd - _FogStart));
                float3 finalColor = lerp(_FogColor.rgb, diffuse, fogFactor);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }

    Fallback "Universal Render Pipeline/Unlit"
}
