Shader "Custom/WorldSpaceUV" {
    Properties {
        _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Scale ("Texture Scale", Float) = 0.1
    }

    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }
        LOD 200

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
                float _Scale;
            CBUFFER_END

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            Varyings vert(Attributes input) {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.worldPos = TransformObjectToWorld(input.positionOS.xyz);
                output.worldNormal = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                float2 uv;
                float3 normal = normalize(input.worldNormal);

                if (abs(normal.x) > 0.5) {
                    uv = input.worldPos.yz;
                } else if (abs(normal.z) > 0.5) {
                    uv = input.worldPos.xy;
                } else {
                    uv = input.worldPos.xz;
                }

                half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * _Scale);

                Light mainLight = GetMainLight();
                half NdotL = saturate(dot(normal, mainLight.direction));
                half3 diffuse = c.rgb * _Color.rgb * mainLight.color * NdotL;

                return half4(diffuse, 1.0);
            }
            ENDHLSL
        }
    }

    Fallback "Universal Render Pipeline/Unlit"
}
