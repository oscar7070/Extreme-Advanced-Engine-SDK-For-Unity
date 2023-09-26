#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class Cloth2DEditorWindow : EditorWindow
{

    private Cloth2D Cloth;

    private Vector2 size = Vector2.one;
    private Vector2Int Subdivision = new(10, 10);

    public void ShowWindow(Cloth2D cloth)
    {
        Cloth = cloth;
        GetWindow<Cloth2DEditorWindow>(true, "PhysFire physics flag editor", true);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("While this window is opend you in edit mode!");
        if (GUILayout.Button("Clear"))
        {
            for (int i = 0; i < Cloth.Geometry.Vertices.Count; i++)
            {
                Cloth2D.ClothGeometry.Vertex vertex = Cloth.Geometry.Vertices[i];
                vertex.Frozen = false;
                Cloth.Geometry.Vertices[i] = vertex;
            }
        }
        if (GUILayout.Button("Fill all"))
        {
            for (int i = 0; i < Cloth.Geometry.Vertices.Count; i++)
            {
                Cloth2D.ClothGeometry.Vertex vertex = Cloth.Geometry.Vertices[i];
                vertex.Frozen = true;
                Cloth.Geometry.Vertices[i] = vertex;
            }
        }
        GUILayout.Label("Mesh creator:");
        GUILayout.BeginVertical();
        int selected = 0;
        string[] options = new string[]
        {
            "CustomMesh", "Rectangle",
        };
        EditorGUILayout.Popup("Type", selected, options);
        //var customMesh = Cloth.CustomMesh;
        //EditorGUILayout.ObjectField( Cloth.GetType().GetField(Cloth.CustomMesh.name));
        EditorGUILayout.LabelField("Subdivision level effects to performance (recomended value: Vec2(10, 10)");
        size = EditorGUILayout.Vector2Field("GeometrySize", size);
        Subdivision = EditorGUILayout.Vector2IntField("SubdivisionLevel", Subdivision);
        if (GUILayout.Button("Generate geomentry"))
        {
            Cloth.ApplyMesh(SimpleMeshCreator.GeneratePlane(Subdivision, size));
        }
        GUILayout.EndVertical();
    }

    private void OnEnable() => SceneView.duringSceneGui += DuringSceneGUI;

    private void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;

    private void DuringSceneGUI(SceneView sceneView)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        if (Selection.gameObjects.Length != 0 && Selection.activeGameObject.GetComponent<Cloth2D>())
        {
            for (int i = 0; i < Cloth.Geometry.Vertices.Count; i++)
            {
                Cloth2D.ClothGeometry.Vertex vertex = Cloth.Geometry.Vertices[i];
                if (vertex.Frozen)
                {
                    Handles.color = Color.red;
                }
                else
                {
                    Handles.color = Color.white;
                }
                if (Handles.Button((Vector3)vertex.position + Cloth.transform.position, Quaternion.identity, .1f, .075f, Handles.DotHandleCap))
                {
                    Undo.RecordObject(Cloth, "Flag2D edit");
                    vertex.Frozen = !vertex.Frozen;
                }
                Handles.color = Color.red;
                if (Handles.Button((Vector3)vertex.position + Cloth.transform.position + (.25f * Vector3.right), Quaternion.identity, .075f, .05f, Handles.RectangleHandleCap))
                {
                    Undo.RecordObject(Cloth, "Flag2D edit");
                    Cloth.Geometry.DeleteVertexAt(i);
                }
                Handles.Label((Vector3)vertex.position + Cloth.transform.position + (.25f * Vector3.down), i.ToString());
                Cloth.Geometry.Vertices[i] = vertex;
            }
        }
        else
        {
            Close();
        }
        SceneView.RepaintAll();
    }
}
#endif