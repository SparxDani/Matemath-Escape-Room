Shader "UI/UnlitPatternURP"
{
    Properties
    {
        _MainTex ("Pattern Sprite", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    _PatternDirection ("Pattern Direction", Vector) = (1,0,0,0)
    _PatternSpeed ("Pattern Speed", Float) = 0.2
    _PatternRotation ("Pattern Rotation (Degrees)", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 100
        Pass
        {
            Name "Unlit"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _PatternDirection;
                float _PatternSpeed;
                float _PatternRotation;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            float2 AnimateUV(float2 uv)
            {
                float2 offset = _PatternDirection.xy * _PatternSpeed * _Time.y;
                float2 movedUV = uv + offset;
                // Rotación en torno al centro (0.5, 0.5)
                float rad = radians(_PatternRotation);
                float2 center = float2(0.5, 0.5);
                float2 rel = movedUV - center;
                float cosR = cos(rad);
                float sinR = sin(rad);
                float2 rotUV = float2(
                    rel.x * cosR - rel.y * sinR,
                    rel.x * sinR + rel.y * cosR
                ) + center;
                return frac(rotUV);
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = AnimateUV(IN.uv);
                half4 texCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                half4 col = texCol * _Color;
                // Mantener la transparencia del sprite y el color
                col.a *= texCol.a;
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Unlit"
}