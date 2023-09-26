Shader "ExtremeEngine/MissingSpriteShader"
{
    Properties
    {
        [NoScaleOffset]_NULLTex ("NULL", 2D) = "White" {}
        [Normal]_NormalTex ("Normal map", 2D) = "bump" {}

    }
    SubShader
    {
        Tags 
        {
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }
        Pass
        {
            Name"FORWARD" 
            Tags
            {
                "LightMode"="ForwardBase"
            }
            
            Cull Off
            Lighting Off
            ZWrite Off
            Blend One OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "LSExLitBase.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };
            
            struct VertexOutput
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _NULLTex;
            float4 _NULLTex_ST;
            sampler2D _NormalTex;

            VertexOutput vert(appdata v)
            {
                VertexOutput o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NULLTex);
                o.color = v.color;
                return o;
            }
            
            fixed4 frag(VertexOutput i) : COLOR
            {
                const fixed2 uVSize = i.uv * 2;
                fixed4 color = tex2D(_NULLTex, uVSize);
                if (CheckIfCanApplyNormals())
                {
                     color.rgb += ApplyNormals(color, i.vertex, uVSize, _NormalTex, 1);
                }
                return color;
            }
            ENDHLSL
        }
    }
    FallBack"Diffuse"
}
