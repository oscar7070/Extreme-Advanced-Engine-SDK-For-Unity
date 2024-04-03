//ExtremeEngine LightSystem2DExtendedLitBase
//You can use this for your own shader just start using it in your shader and like if you want to Add normals just add a normal map texture property and ApplyNormals() to your color.
//Exemple of usage:

//if (CheckIfCanApplyNormals()) 
//{
//    color.rgb += ApplyNormals(color, i.vertex, i.uv, _NormalTex, _NormalsIntensity);
//}

#include "UnityCG.cginc"
//Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
//#pragma exclude_renderers gles

uniform vector _LSLightPos;
uniform int NormalMapsEnabled; //1 is enabled.

static fixed3 ApplyNormals(fixed3 color, float2 vertex, float2 uv, sampler2D normalTex, fixed normalsIntensity)
{
    fixed2 direction = vertex -_LSLightPos.xy;
    fixed3 normals = UnpackNormal(tex2D(normalTex, uv));
    fixed3 lNormals = normalize(fixed3(-direction.x, -direction.y, 0));
    fixed normStr = dot(normals, lNormals);
    fixed ref = max(.5f, reflect(-lNormals, normals).z);
    return normStr * ref * normalsIntensity * color;
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

static float2 Scale(float2 uv, float2 scale) 
{
    return (uv - 0.5) / scale + 0.5;
}

static float3 AddLayer(float3 color, float3 add, float addAlpha = 1) 
{
    return lerp(color.rgb, add, addAlpha);
}

static float3 RotateAboutAxis(float3 In, float3 Axis, float Rotation)
{
    Rotation = radians(Rotation);
    float s = sin(Rotation);
    float c = cos(Rotation);
    float one_minus_c = 1.0 - c;

    Axis = normalize(Axis);
    float3x3 rot_mat =
    {
        one_minus_c * Axis.x * Axis.x + c, one_minus_c * Axis.x * Axis.y - Axis.z * s, one_minus_c * Axis.z * Axis.x + Axis.y * s,
        one_minus_c * Axis.x * Axis.y + Axis.z * s, one_minus_c * Axis.y * Axis.y + c, one_minus_c * Axis.y * Axis.z - Axis.x * s,
        one_minus_c * Axis.z * Axis.x - Axis.y * s, one_minus_c * Axis.y * Axis.z + Axis.x * s, one_minus_c * Axis.z * Axis.z + c
    };
    return mul(rot_mat, In);
}

inline float unity_noise_randomValue(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

inline float unity_noise_interpolate(float a, float b, float t)
{
    return (1.0 - t) * a + (t * b);
}

inline float unity_valueNoise(float2 uv)
{
    float2 i = floor(uv);
    float2 f = frac(uv);
    f = f * f * (3.0 - 2.0 * f);

    uv = abs(frac(uv) - 0.5);
    float2 c0 = i + float2(0.0, 0.0);
    float2 c1 = i + float2(1.0, 0.0);
    float2 c2 = i + float2(0.0, 1.0);
    float2 c3 = i + float2(1.0, 1.0);
    float r0 = unity_noise_randomValue(c0);
    float r1 = unity_noise_randomValue(c1);
    float r2 = unity_noise_randomValue(c2);
    float r3 = unity_noise_randomValue(c3);

    float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
    float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
    float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
    return t;
}

static float SimpleNoise(float2 UV, float Scale)
{
    float t = 0.0;

    float freq = pow(2.0, float(0));
    float amp = pow(0.5, float(3 - 0));
    t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

    freq = pow(2.0, float(1));
    amp = pow(0.5, float(3 - 1));
    t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

    freq = pow(2.0, float(2));
    amp = pow(0.5, float(3 - 2));
    t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

    return t;
}

float2 unity_gradientNoise_dir(float2 p)
{
    p = p % 289;
    float x = (34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

float unity_gradientNoise(float2 p)
{
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(unity_gradientNoise_dir(ip), fp);
    float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
}

float GradientNoise(float2 UV, float Scale)
{
    return unity_gradientNoise(UV * Scale) + 0.5;
}

inline float2 unity_voronoi_noise_randomVector(float2 UV, float offset)
{
    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
    UV = frac(sin(mul(UV, m)) * 46839.32);
    return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
}

float Voronoi(float2 UV, float AngleOffset, float CellDensity)
{
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);

    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x, y);
            float2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);
            if (d < res.x)
            {
                res = float3(d, offset.x, offset.y);
            }
        }
    }
    return res.x;
}

float VoronoiCells(float2 UV, float AngleOffset, float CellDensity)
{
    float2 g = floor(UV * CellDensity);
    float2 f = frac(UV * CellDensity);
    float t = 8.0;
    float3 res = float3(8.0, 0.0, 0.0);

    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x, y);
            float2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);
            if (d < res.x)
            {
                res = float3(d, offset.x, offset.y);
            }
        }
    }
    return res.y;
}