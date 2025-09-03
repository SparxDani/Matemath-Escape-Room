Shader "UI/UnlitPatternWebGL"
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
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _PatternDirection;
            float _PatternSpeed;
            float _PatternRotation;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float2 AnimateUV(float2 uv)
            {
                float2 offset = _PatternDirection.xy * _PatternSpeed * _Time.y;
                float2 movedUV = uv + offset;
                // Rotación en torno al centro (0.5, 0.5)
                float rad = _PatternRotation * UNITY_PI / 180.0;
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

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = AnimateUV(i.uv);
                fixed4 texCol = tex2D(_MainTex, uv);
                fixed4 col = texCol * _Color;
                col.a *= texCol.a;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Unlit"
}