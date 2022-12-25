Shader "Hiden/SkyShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "black" {}

        [Header(Sky)]
        [NoScaleOffset] _SkyVerticalGradient("Sky vertical gradient", 2D) = "white" {}
        _SkyVerticalGradientDistance("Sky vertical gradient distance", Range(0, 10)) = 5
        _SkyUpColor("Sky up color", color) = (1,1,1,1)
        _SkyDownColor("Sky down color", color) = (.75, .75, .75,1)

        [Header(Clouds)]
        [NoScaleOffset] _CloudsNoiseTex ("Clouds noise texture", 2D) = "black" {}
        _CloudsColor("Clouds color", color) = (1,1,1,1)
        _CloudsIntensity("Clouds intensity", Range(0, 10)) = 1
        _CloudsHight("Clouds hight", Range(-1, 1)) = 1
        _CloudsSoftness("Clouds softness", Range(0, 1)) = 1
        _CloudsCoverage("Clouds coverage", Range(0, 1)) = 0.5
        _CloudsDistance("Clouds distance",  Range(0, 1)) = .25
        _CloudsSpeed("Clouds speed", float) = 1

        [Header(Sun)]
        [NoScaleOffset] _SunTex("Sun texture", 2D) = "black" {}
        _SunItensity("Sun itensity", Range(1, 10)) = 5
        _SunSize("Sun size",  Range(0, 1.5)) = 0.1
        _SunPosition("Sun position", vector) = (0,0,0,0)

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        ZClip False

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "LSExLitBase.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VertexOutput
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            VertexOutput vert (appdata v)
            {
                VertexOutput o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _SkyVerticalGradient;
            fixed _SkyVerticalGradientDistance;
            fixed3 _SkyUpColor;
            fixed3 _SkyDownColor;
            sampler2D _CloudsNoiseTex;
            fixed3 _CloudsColor;
            fixed _CloudsIntensity;
            fixed _CloudsHight;
            fixed _CloudsSoftness;
            fixed _CloudsCoverage;
            fixed _CloudsSpeed;
            fixed _CloudsDistance;
            sampler2D _SunTex;
            fixed _SunItensity;
            fixed _SunSize;
            fixed2 _SunPosition;

            fixed Noise(sampler2D noiseTex, float2 uv, fixed2 direction, float hight)
            {
                float2 noiseUV = uv;
                noiseUV.x -= 0.5;
                noiseUV /= 1.0 - noiseUV.y * 2.0;
                noiseUV.x += 0.5;
                noiseUV.x += _Time * 5;
                noiseUV = TilingAndOffset(noiseUV, _CloudsDistance, fixed2(-_CloudsDistance, -_CloudsDistance));

                noiseUV.x += direction.x * (_Time * RandomRange(-_CloudsSpeed, _CloudsSpeed));
                noiseUV.y += direction.y * (_Time * RandomRange(-_CloudsSpeed, _CloudsSpeed));
                noiseUV.y -= hight * _CloudsHight;
                fixed color = -tex2D(noiseTex, noiseUV);
                color += hight;
                return color;
            }

            fixed4 frag (VertexOutput i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed2 skyUV = i.uv * _SkyVerticalGradientDistance;
                skyUV.y -= _SkyVerticalGradientDistance - .5;
                skyUV.y += _SkyVerticalGradientDistance * .5;
                fixed skyVerticalGradient = tex2D(_SkyVerticalGradient, skyUV);
                fixed skyVerticalCut = tex2D(_SkyVerticalGradient, i.uv);
                fixed4 finalNoise;
                const fixed iterations = 50;
                for (int n = 0; n < iterations; n++)
                {
                    const fixed progress = n / iterations;
                    const fixed hight = lerp(0, _CloudsSoftness, progress);
                    fixed noiseUnit = Noise(_CloudsNoiseTex, i.uv, fixed2(1, 1), hight) * Noise(_CloudsNoiseTex, i.uv, fixed2(-1, -1), hight) * Noise(_CloudsNoiseTex, i.uv, fixed2(-1, 1), hight) * Noise(_CloudsNoiseTex, i.uv, fixed2(1, -1), hight);
                    noiseUnit = min(noiseUnit * 8, 1);
                    noiseUnit = step(1 - _CloudsCoverage, noiseUnit);
                    noiseUnit = noiseUnit / iterations;
                    noiseUnit *= 1 - abs(lerp(-1, 1, n / iterations));
                    //noiseUnit = pow(noiseUnit, _CloudsSoftness);
                    //noiseUnit *= lerp(0, 1, progress);
                    finalNoise.a += noiseUnit;
                    finalNoise.rgb = AddLayer(finalNoise.rgb,_CloudsColor, noiseUnit);
                }
                float2 sunUV = Scale(i.uv - _SunPosition, _SunSize);
                fixed4 sun = tex2D(_SunTex, sunUV);
                finalNoise *= _CloudsIntensity;
                col.rgb = AddLayer(col.rgb, lerp(_SkyDownColor, _SkyUpColor, skyVerticalGradient));
                col.rgb = AddLayer(col.rgb, _SunItensity, sun.rgb);
                col.rgb = AddLayer(col.rgb, finalNoise.rgb, finalNoise.a * skyVerticalGradient);
                return col;
            }
            ENDHLSL
        }
    }
}
