Shader "Hidden/FogShader"
{
    Properties
    {
        _MainTex("Main texture", 2D) = "black" {}
        _NoiseTex ("Noise", 2D) = "black" {}
        _NoiseIntensity("Noise intensity", float) = .5
        _NoiseSize ("Noise size", float) = .5
        _NoiseColor("Noise color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //float2 noiseUV : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //float2 noiseUV : TEXCOORD1;
                //float2 noiseUV : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            fixed _NoiseIntensity;
            fixed _NoiseSize;
            fixed3 _NoiseColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv += frac(_MainTex_ST.xy * _Time.yy);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed2 size = normalize(unity_OrthoParams.xy);
                //size = (size.x + size.y) * .5;
                fixed2 noiseUV = ((i.uv - (.5, .5)) / (_NoiseSize)) * (size);
                //noiseUV += _WorldSpaceCameraPos.xy;
                noiseUV.x += _Time;
                fixed4 noise = tex2D(_NoiseTex, noiseUV);
                col.rgb += ((col.r * .3 + col.g * .59 + col.b * .11) * noise.a * _NoiseIntensity) * _NoiseColor;
                return col;
            }
            ENDCG
        }
    }
}
