#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Light2DSettings))]
public class Light2DSettingsCustomInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Light2DSettings light2DSettings = (Light2DSettings)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Light System2D info:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(light2DSettings.Version);
    }
}
#endif