Shader"Skybox/ExtremeEngineSky"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "black" {}

        [Header(Sky)]
        [NoScaleOffset] _SkyVerticalGradient("Sky vertical gradient", 2D) = "white" {}
        _SkyVerticalGradientDistance("Sky vertical gradient distance", Range(0, 10)) = 5
        [HDR]_SkyUpColor("Sky up color", color) = (1,1,1,1)
        [HDR]_SkyDownColor("Sky down color", color) = (.75, .75, .75,1)
        _SkyRotationX("Sky rotation x",Range(0,360)) = 0
        _SkyRotationY("Sky rotation y",Range(0,360)) = 0
        [NoScaleOffset] _SkyStarsTex ("Sky stars tex", 2D) = "black" {}
        [HDR]_SkyStarsColor("SkyStars Color", color) = (1,1,1,1)

        [Header(Clouds)]
        [NoScaleOffset] _CloudsNoiseTex ("Clouds noise texture", 2D) = "black" {}
        _CloudsNoiseScale ("Clouds noise scale", float) = 1
        [NoScaleOffset] _CloudsDetailTex ("Clouds detail texture", 2D) = "black" {}
        _CloudsDetailScale ("Clouds detail scale", float) = 1
        _CloudsColor("Clouds color", color) = (1,1,1,1)
        _CloudsDarknessColor("Clouds darkness color", color) = (.75,.75,.75,1)
        _CloudsIntensity("Clouds intensity", Range(0, 2)) = 1
        _CloudsHight("Clouds hight", Range(0, 12)) = 1
        _CloudsCoverage("Clouds coverage", Range(0, 1)) = 0.5
        _CloudsDistance("Clouds distance",  Range(0, 10)) = 6
        _CloudsDirection("Clouds direction", vector) = (0,0,0,0)
        _CloudsSpeed("Clouds speed", float) = 1

        [Header(Sun)]
        [NoScaleOffset] _SunTex("Sun texture", 2D) = "black" {}
        _SunItensity("Sun itensity", Range(1, 10)) = 5
        _SunSize("Sun size",  Range(0, 100)) = 0.1
        _SunPosition("Sun position", vector) = (0,0,0,0)

        [Header(Moon)]
        [NoScaleOffset] _MoonTex("Moon texture", 2D) = "black" {}
        [NoScaleOffset] _MoonAppearanceTex("Moon appearance texture", 2D) = "black" {}
        [HDR]_MoonColorAppearanceSide("Moon color appearance side", color) = (1,1,1,1)
        [HDR]_MoonColorDarkSide("Moon color dark side", color) = (1,1,1,1)
        _MoonSize("Moon size",  Range(0, 100)) = 0.1
        _MoonPosition("Moon position", vector) = (0,0,0,0)
        _MoonAppearancePosition("Moon appearance position", vector) = (0,0,0,0)
        _MoonAppearanceScale("Moon appearance scale", vector) = (0,0,0,0)

    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull off
ZWrite Off
ZTest Always
        Fog{Mode Off}  
        ZClip False

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "LSExLitBase.cginc"

fixed _SkyRotationX;
fixed _SkyRotationY;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct VertexOutput
            {
                float3 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 uvRotated : TEXCOORD2;
                float4 vertex : SV_POSITION;
                //float4 cameraPosition : TEXCOORD3;
};

            VertexOutput vert (appdata v)
            {
                VertexOutput o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvRotated = RotateAboutAxis(v.vertex, fixed3(1, 0, 0), _SkyRotationX);
                o.uv = v.uv;
                //o.cameraPosition = mul(unity_WorldToObject, float4(v.vertex, 1.0));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            sampler2D _MainTex;

            sampler2D _SkyVerticalGradient;
            fixed _SkyVerticalGradientDistance;
            fixed3 _SkyUpColor;
            fixed3 _SkyDownColor;
            sampler2D _SkyStarsTex;
            fixed4 _SkyStarsColor;

            sampler2D _CloudsNoiseTex;
            float _CloudsNoiseScale;
            sampler2D _CloudsDetailTex;
float _CloudsDetailScale;
            fixed3 _CloudsColor;
            fixed3 _CloudsDarknessColor;
            fixed3 _MoonColorAppearanceSide;
            fixed3 _MoonColorDarkSide;
            fixed _CloudsIntensity;
            fixed _CloudsHight;
            fixed _CloudsCoverage;
            fixed _CloudsSpeed;
            fixed _CloudsDistance;
            fixed2 _CloudsDirection;

            sampler2D _SunTex;
            fixed _SunItensity;
            fixed _SunSize;
            fixed2 _SunPosition;

            sampler2D _MoonTex, _MoonAppearanceTex;
            fixed _MoonSize;
            fixed2 _MoonPosition;
fixed2 _MoonAppearancePosition;
fixed2 _MoonAppearanceScale;

            static fixed Noise(fixed noiseScale, fixed detailScale, float2 uv, fixed2 directionToAdd, float3 worldSpaceCameraPosCurrentDis, fixed hight)
            {
                uv = TilingAndOffset(uv, ((_CloudsDistance + (_CloudsHight * .5)) + ((hight - .5) * _CloudsHight)) - (worldSpaceCameraPosCurrentDis.y), fixed2(worldSpaceCameraPosCurrentDis.x, worldSpaceCameraPosCurrentDis.z));
                uv.x += directionToAdd.x;
                uv.y += directionToAdd.y;
                fixed color = GradientNoise(uv, noiseScale) * SimpleNoise(uv, detailScale);
                color += hight;
                return color;
            }

            static fixed4 Object(sampler2D Tex, float2 uv, float2 position, float2 scale)
            {
                float2 UVs = Scale(uv - position, scale);
                return tex2D(Tex, UVs);
            }

            fixed4 frag (VertexOutput i) : SV_Target
            {
                float2 SkyUV = -(i.worldPos.xz / i.worldPos.y) + fixed2(.5,.5);
                float2 SkyUVRotated = (i.uvRotated.xz / i.uvRotated.y) + fixed2(.5, .5);
                fixed j = 1 - i.uv.y;
                fixed jRotated = 1 - i.uvRotated.y;
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed d = dot(normalize(i.uvRotated), fixed2(0, 1)) * .5 + .5;
                col.rgb = AddLayer(col.rgb, lerp(_SkyUpColor, _SkyDownColor, pow(d, 1)), 1);

                fixed4 stars = tex2D(_SkyStarsTex, i.uv) * _SkyStarsColor;
                col.rgb = AddLayer(col.rgb, stars.rgb, lerp(stars.a, 0, col.rgb));
                if (jRotated > 1)
                {
                    fixed4 sun = Object(_SunTex, SkyUVRotated, _SunPosition, _SunSize);
                    col.rgb = AddLayer(col.rgb, sun.rgb * _SunItensity, sun.a);
                }
                else
                {
        fixed2 moonAppearancePosition = _MoonPosition + (_MoonAppearancePosition * _MoonSize);                   
        fixed moonAppearnce = Object(_MoonAppearanceTex, SkyUVRotated, moonAppearancePosition, _MoonSize * _MoonAppearanceScale);
                    fixed4 moon = Object(_MoonTex, SkyUVRotated, _MoonPosition, _MoonSize);
                        col.rgb = AddLayer(col.rgb, lerp(moon.rgb * _MoonColorAppearanceSide, moon.rgb * _MoonColorDarkSide, clamp(moonAppearnce * 2, 0, 1)), moon.a);
                }
                if (_CloudsCoverage > 0 & j < .99)
                {
                    fixed cloudsTransition =  1 - i.uv.y * 2;
                    const fixed iterations = 32;
                    const fixed3 worldSpaceCameraPosCurrentDistance = _WorldSpaceCameraPos * .001;
                    const fixed2 MoveToDirection = _CloudsDirection * _Time * _CloudsSpeed;
                    for (int n = 0; n < iterations; n++)
                    {
                        const fixed progress = n / iterations;
                        const fixed hight = lerp(0, 1, progress);
                        fixed noiseUnit = Noise(_CloudsNoiseScale,_CloudsDetailScale, SkyUV, MoveToDirection, worldSpaceCameraPosCurrentDistance, hight);
                        noiseUnit = step(noiseUnit, _CloudsCoverage * 2);
                        noiseUnit = noiseUnit / iterations;
                        noiseUnit = lerp(noiseUnit, 0, cloudsTransition);
                        col.rgb = AddLayer(col.rgb, lerp(_CloudsColor, _CloudsDarknessColor, progress), noiseUnit * _CloudsIntensity);
                    }
                }
                return col;
            }
            ENDHLSL
        }
    }
}
