Shader "Lit2D/MarkedObjectLit2DShader"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Sprite", 2D) = "White" {}
        [hdr]_MarkerColor("Marker color", Color) = (1,1,1,1)
        [NoScaleOffset]_MarkerTex ("Marker sprite", 2D) = "black" {}
        [Normal]_NormalTex ("Normal map", 2D) = "bump" {}
        _NormalsIntensity("Normals intensity", float) = 1
        [NoScaleOffset]_EmissionTex ("Emission map", 2D) = "black" {}
        [hdr]_EmissionColor ("Emission color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }
        Pass
        {
            Name "FORWARD"
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
                float2 uvMarker : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct VertexOutput
            {
                float2 uv : TEXCOORD0;
                float2 uvMarker : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _MarkerColor;
            sampler2D _MarkerTex;
            float4 _MarkerTex_ST;
            sampler2D _NormalTex;
            fixed _NormalsIntensity;
            sampler2D _EmissionTex;
            fixed3 _EmissionColor;
                    
            //World Properties
            struct LightUnit
            {
                vector position;
                float size;
            };

            //uniform LightUnit Lights[1];

            uniform RWStructuredBuffer<LightUnit> _LSLightsBuffer;

            //uniform vector _LSLightPos;
            uniform float _LSLightSize;
            uniform texture2D _LSLightsTex;
            uniform matrix _LSLightsMatrix;
            SamplerState sampler_LSLightsTex;

            VertexOutput vert (appdata v)
            {
                VertexOutput o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvMarker = TRANSFORM_TEX(v.uvMarker, _MarkerTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (VertexOutput i) : COLOR
            {
                fixed3 emissionTex = tex2D(_EmissionTex, i.uv);
                fixed4 color = tex2D(_MainTex, i.uv);
                color.a *= i.color.a;
                color.rgb *= i.color.rgb * color.a;
                fixed4 markerTex = tex2D(_MarkerTex, i.uvMarker - (fixed2(_Time.y, _Time.y))) * _MarkerColor;
                color.rgb += markerTex.rgb * markerTex.a * color.a;
                if (CheckIfCanApplyNormals()) 
                {
                    color.rgb += ApplyNormals(color, i.vertex, i.uv, _NormalTex, _NormalsIntensity);
                }
                color.rgb += emissionTex * _EmissionColor;
                //color.rgb += ApplySnow(color, i.vertex, i.uv, _NormalTex);
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
