#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Light2DCollider)), CanEditMultipleObjects]
public class Light2DColliderCustomInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Light2DCollider light2DCollider = (Light2DCollider)target;
        Undo.RecordObject(light2DCollider, "Light2DCollider");
        var colliderParams = light2DCollider.Params;
        var polygonColliders = light2DCollider.GetComponents<PolygonCollider2D>();
        if (polygonColliders.Length != 0)
        {
            string[] colliderOptions = new string[polygonColliders.Length];
            for (int i = 0; i < colliderOptions.Length; i++)
            {
                colliderOptions[i] = "PolygonCollider " + i;
            }
            colliderParams.Selected = EditorGUILayout.Popup("Poly Coll to copy", colliderParams.Selected, colliderOptions);
            string[] pathOptions = new string[polygonColliders[colliderParams.Selected].pathCount];
            for (int i = 0; i < pathOptions.Length; i++)
            {
                pathOptions[i] = i.ToString();
            }
            light2DCollider.Params.SelectedPath = EditorGUILayout.Popup("Path", colliderParams.SelectedPath, pathOptions);

            colliderParams.CopyOnChange = GUILayout.Toggle(colliderParams.CopyOnChange, "CopyOnChange");
            if (colliderParams.CopyOnChange & !Application.isPlaying)
            {
                light2DCollider.CopyPointsFromPolygonCollider(polygonColliders[colliderParams.Selected], colliderParams.SelectedPath);
            }
            else if (GUILayout.Button("Copy"))
            {
                light2DCollider.CopyPointsFromPolygonCollider(polygonColliders[colliderParams.Selected], colliderParams.SelectedPath);
            }
        }
    }
}
#endif
