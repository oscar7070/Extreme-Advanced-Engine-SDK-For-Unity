#if UNITY_EDITOR
/*using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), ID, "Light system2D overlay")]
public class Light2DOverlay : ToolbarOverlay
{
    public const string ID = "LightSystem2DOverlay";

    public Light2DOverlay() : base(Light2DOverlayLightEnebledToggle.ID) { }
}

[EditorToolbarElement(ID, typeof(SceneView))]
public class Light2DOverlayLightEnebledToggle : EditorToolbarToggle
{

    public const string ID = "LightSystem2DOverlay/Toggle";

    public Light2DOverlayLightEnebledToggle()
    {
        value = true;
        text = "Light2D Enabled";
        offIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(ExtremeEngineData.GetEngineFolderPath + "Icons/Light2DDisabledOverlayLogo.png");
        onIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(ExtremeEngineData.GetEngineFolderPath + "Icons/Light2DOverlayLogo.png");
        tooltip = "Set light enabled in editor";
        this.RegisterValueChangedCallback(Callback);
    }

    private void Callback(ChangeEvent<bool> evt)
    {
        Light2DCameraRenderer[] light2DCameraRenderers = Object.FindObjectsOfType<Light2DCameraRenderer>();
        for (int i = 0; i < light2DCameraRenderers.Length; i++)
        {
            light2DCameraRenderers[i].SetLightEnebledInSceneView(!evt.newValue);
        }
    }
}*/
#endif