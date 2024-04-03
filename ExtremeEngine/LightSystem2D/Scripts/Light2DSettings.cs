using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Light2DSettings", menuName = "2D/Light2DSettings")]
public class Light2DSettings : ScriptableObject
{

    [SerializeField] private Material LightMaterial;
    [HideInInspector] public Material LightMat => LightMaterial;


    [SerializeField] private Material ShadowMaterial;
    [HideInInspector] public Material ShadowMat => ShadowMaterial;


    [SerializeField] private Material GlobalLightMaterial;
    [HideInInspector] public Material GlobalLightMat => GlobalLightMaterial;


    [SerializeField] private Material FinalImageWithLightMaterial;
    [HideInInspector] public Material FinalImageWithLightMat => FinalImageWithLightMaterial;

    [SerializeField] private Material NormalsMaterial;
    [HideInInspector] public Material NormalsMat => NormalsMaterial;

    [SerializeField] private Material FogMaterial;
    [HideInInspector] public Material FogMat => FogMaterial;

    [Header("Options")]
    [SerializeField, Range(0.0001f, 1)] private float LightResolution = .5f;
    private readonly string LightResolutionPrefsKey = "Light2DResolution";

    public void LoadAllSettings()
    {
        LightResolution = PlayerPrefs.GetFloat(LightResolutionPrefsKey, .75f);
        MaximumDrawenLights = PlayerPrefs.GetInt(MaximumDrawenLightsPrefsKey, 24);

        string normals = PlayerPrefs.GetString(MaximumDrawenLightsPrefsKey, "true");
        if (normals == "true")
        {
            NormalMaps = NormalMapsProperties.On;
        }
        else
        {
            NormalMaps = NormalMapsProperties.Off;
        }
        Shader.SetGlobalInt("NormalMapsEnabled", (int)GetNormalMapsEnabled());
    }

    public void SetLightResolution(float resolution)
    {
        PlayerPrefs.SetFloat(LightResolutionPrefsKey, resolution);
        LightResolution = resolution;
    }

    public float GetLightResolution()
    {
        return LightResolution;
    }

    [SerializeField, Min(0)] private int MaximumDrawenLights = 24;
    private readonly string MaximumDrawenLightsPrefsKey = "Light2DMaximumDrawenLights";

    public void SetMaximumDrawenLights(int maximumDrawenLights)
    {
        PlayerPrefs.SetInt(MaximumDrawenLightsPrefsKey, maximumDrawenLights);
        MaximumDrawenLights = maximumDrawenLights;
    }

    public int GetMaximumDrawenLights()
    {
        return MaximumDrawenLights;
    }

    public void GenerateGetMaximumDrawenLightDropdown(Dropdown dropdown)
    {
        List<Dropdown.OptionData> options = new();
        for (int i = 0; i < 28 + 1; i++)
        {
            options.Add(new Dropdown.OptionData(i.ToString()));
        }
        dropdown.AddOptions(options);
        dropdown.value = GetMaximumDrawenLights();
    }

    public enum NormalMapsProperties
    {
        Off,
        On,
    }

    [SerializeField] private NormalMapsProperties NormalMaps = NormalMapsProperties.On;
    private readonly string EnableNormalMapsPrefsKey = "Light2DEnableNormalMaps";

    public NormalMapsProperties GetNormalMapsEnabled()
    {
        return NormalMaps;
    }

    public void SetNormalMapsEnabled(bool enabled)
    {
        if (enabled)
        {
            NormalMaps = NormalMapsProperties.On;
        }
        else
        {
            NormalMaps = NormalMapsProperties.Off;
        }
        PlayerPrefs.SetString(EnableNormalMapsPrefsKey, enabled.ToString());
        Shader.SetGlobalInt("NormalMapsEnabled", (int)GetNormalMapsEnabled());
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Shader/Light System Extended Shader")]
    private static void CreateDefaultLSExtShader()
    {
        Shader shader = Shader.Find("Lit2D/StandartLit2DShader");
        Debug.Log(shader);

        if (shader)
        {
            AssetDatabase.CreateAsset(shader, AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));
        }
        else
        {
            Debug.Log("The default Light system shader not found: LSExLitShader.shader");
        }
    }
#endif
}
