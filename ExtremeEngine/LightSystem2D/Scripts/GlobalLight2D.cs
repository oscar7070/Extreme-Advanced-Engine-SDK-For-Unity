using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("Light2D/Global light2D")]
public class GlobalLight2D : MonoBehaviour
{

    [SerializeField, ColorUsage(false)] private Color Color = Color.white;
    public bool UseColorTemperture = false;
    [Range(1000, 20000)] public float ColorTemperature = 6570;
    [HideInInspector] public Color FinalColor => GetFinalColor();
    [Min(0)] public float Intensity = 1;
    [SerializeField] private FogSettings Fog;
    [SerializeField] private SkySettings Sky;
    [HideInInspector] public Light2DSettings Light2DSettings;
    private Material GlobalLightPostEffectMaterial;
    private Material FogPostEffectMaterial;

    [Serializable]
    private class FogSettings
    {

        [Min(0)] public float Intensity = 0;
        [Min(0)] public float Size = 1;
        public Color Color = Color.white;
    }

    [Serializable]
    private class SkySettings
    {
       [Header("Time shown in PM")]
       [Range(0, 24)] public float DayTime = 12;
       [GradientUsage(true)] public Gradient DayNightCycle = new();
    }

    #region Set on render image callback
    private void OnEnable()
    {
        Light2DCameraRenderer.OnRenderImageCallbackForLight += OnCameraRenderImage;
    }

    private void OnDisable()
    {
        Light2DCameraRenderer.OnRenderImageCallbackForLight -= OnCameraRenderImage;
    }

    private void OnDestroy()
    {
        Light2DCameraRenderer.OnRenderImageCallbackForLight -= OnCameraRenderImage;
    }
    #endregion

    private void Awake()
    {
        GlobalLightPostEffectMaterial = new Material(Light2DSettings.GlobalLightMat.shader);
        GlobalLightPostEffectMaterial.CopyPropertiesFromMaterial(Light2DSettings.GlobalLightMat);
        FogPostEffectMaterial = new Material(Light2DSettings.FogMat.shader);
        FogPostEffectMaterial.CopyPropertiesFromMaterial(Light2DSettings.FogMat);
    }

    private Color GetFinalColor()
    {
        if (UseColorTemperture)
        {
            return Mathf.CorrelatedColorTemperatureToRGB(ColorTemperature) * Color;
        }
        else
        {
            return Color;
        }
    }

    private void OnCameraRenderImage(RenderTexture source, Light2DCameraRenderer light2DCameraRenderer, Camera camera, List<Light2DCollider> colliders, int maxDrawenLigths)
    {
        GlobalLightPostEffectMaterial.SetTexture("_MainTexture", source);
        GlobalLightPostEffectMaterial.SetFloat("_Intensity", Intensity);
        GlobalLightPostEffectMaterial.SetColor("_Color", FinalColor);
        light2DCameraRenderer.GlobalLightIntensity += Intensity;
        light2DCameraRenderer.BlitLight(new Light2DCameraRenderer.BlitLightSettings(GlobalLightPostEffectMaterial));
        if (Fog.Intensity != 0)
        {
            FogPostEffectMaterial.SetTexture("_MainTex", source);
            FogPostEffectMaterial.SetFloat("_NoiseIntensity", Fog.Intensity);
            FogPostEffectMaterial.SetFloat("_NoiseSize", Fog.Size);
            FogPostEffectMaterial.SetColor("_NoiseColor", Fog.Color);
            light2DCameraRenderer.BlitLight(new Light2DCameraRenderer.BlitLightSettings(FogPostEffectMaterial, 3));
        }
    }

    public void SetColor(Color colorRGB)
    {
        Color = new Color(colorRGB.r, colorRGB.g, colorRGB.b);
    }

    public Color GetColor()
    {
        return Color;
    }
}
