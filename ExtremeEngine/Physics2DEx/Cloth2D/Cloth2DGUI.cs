#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cloth2D)), CanEditMultipleObjects]
public class Cloth2DGUI : Editor
{

    public override void OnInspectorGUI()
    {
        Cloth2D cloth = (Cloth2D)target;
        if (GUILayout.Button("Generate geometry"))
        {
            Undo.RecordObject(cloth, "cloth Generate geometry");
            cloth.ApplyMesh(SimpleMeshCreator.GeneratePlane(new(10, 10), Vector2.one));
        }
        if (GUILayout.Button("Open cloth editor"))
        {
            var f = CreateInstance<Cloth2DEditorWindow>();
            f.ShowWindow(cloth);
        }
        DrawDefaultInspector();
    }
}
#endif