Shader "Unlit/LSExNormal"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Sprite", 2D) = "White" {}

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
Name"FORWARD"
            Tags
{
                "LightMode"="ForwardBase"
}

Cull Off

Lighting Off

ZWrite Off

Blend One
OneMinusSrcAlpha
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

sampler2D _MainTex;
float4 _MainTex_ST;
                  
VertexOutput vert(appdata v)
{
    VertexOutput o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.color = v.color;
    return o;
}

fixed3 frag(VertexOutput i) : COLOR
{
    fixed3 color = UnpackNormal(tex2D(_MainTex, i.uv));
    return color;
}
            ENDHLSL
        }
    }
FallBack"Diffuse"
}
