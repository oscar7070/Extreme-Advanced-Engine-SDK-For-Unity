#include "UnityCG.cginc"

uniform vector _LSLightPos;
uniform int NormalMapsEnabled; //1 is enabled.

static fixed3 ApplyNormals(fixed4 color, float2 vertex, float2 uv, sampler2D normalTex, fixed normalsIntensity)
{
    fixed2 direction = vertex - _LSLightPos.xy;
    fixed3 normals = UnpackNormal(tex2D(normalTex, uv));
    fixed3 lNormals = normalize(fixed3(-direction.x, direction.y, 0));
    fixed normStr = dot(normals, lNormals);
    fixed ref = max(0, reflect(-lNormals, normals).z);
    return (normStr * ref * normalsIntensity) * color.a;
}

static bool CheckIfCanApplyNormals() 
{
    if (NormalMapsEnabled == 1)
    {
        return true;
    }
    else 
    {
        return false;
    }
}

static fixed3 ApplySnow(fixed4 color, float2 vertex, float2 uv, sampler2D normalTex)
{
    fixed3 normals = UnpackNormal(tex2D(normalTex, uv));
    fixed3 snowColor = max(0, reflect(-fixed3(0, 1, 0), normals).z) * color.a;
    for (int i = 1; i < 5; i++)
    {
        //snowColor += max(0, reflect(-fixed3(0, 1, 0), UnpackNormal(tex2D(normalTex, uv - fixed2(0, i * .001)))).z) * color.a;
    }
    snowColor = min(1, snowColor);
    return snowColor;
}

static float3 Remap(float3 In, float2 InMinMax, float2 OutMinMax)
{
    return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

static float RandomRange(float Min, float Max, float Seed = 0)
{
    float randomno = frac(sin(dot(Seed, float2(12.9898, 78.233))) * 43758.5453);
    return lerp(Min, Max, randomno);
}

static float2 TilingAndOffset(float2 UV, float2 Tiling, float2 Offset)
{
    return UV * Tiling + Offset;
}

static float2 Scale(float2 uv, float scale) 
{
    return (uv - 0.5) / scale + 0.5;
}

static float3 AddLayer(float3 color, float3 add, float addAlpha = 1) 
{
    return lerp(color.rgb, add, addAlpha);
}