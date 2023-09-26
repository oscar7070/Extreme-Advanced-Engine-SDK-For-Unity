#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Light2D)), CanEditMultipleObjects()]
public class Light2DCustomInspector : Editor
{

    public override void OnInspectorGUI()
    {
        Light2D light2D = (Light2D)target;
        base.OnInspectorGUI();
        if (light2D.LightMode == Light2D.LightModeEnum.BakedOnStart)
        {
            if (GUILayout.Button("Bake"))
            {

            }
        }
    }
}
#endif