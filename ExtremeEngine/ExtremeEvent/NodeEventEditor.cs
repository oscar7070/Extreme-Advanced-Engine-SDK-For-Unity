#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode()]//[Icon(AssetDatabase.getpath( Icon))]
public class NodeEventEditor : EditorWindow
{

    public Texture Icon;
    public Texture PropertyIcon;
    public Texture ConnectPointInButtonIcon;
    public Texture ConnectPointOutButtonIcon;
    public Texture CloseButtonIcon;
    public Texture FieldIcon;
    public Texture MethodIcon;
    private readonly Vector2 MinWindowSize = new(400, 100);
    public List<ExtremeEventNode> Nodes = new();
    public ExtremeEventNode.NodeCurve SelectedCurve = null;
    public readonly List<ExtremeEventNode.NodeCurve> Curves = new();
    private Vector2 MousePosition;
    private const float EditorActiveAreaX = 1920, EditorActiveAreaY = 1080;

    public static NodeEventEditor Create()
    {
        return GetWindow<NodeEventEditor>();
    }

    public void Run(SerializedProperty from)
    {
        titleContent = new(new GUIContent("ExtremeEvent", Icon)/*editor<" + from.name + ">"*/);
        maximized = true;
        TryCreateNewNodeWindow((from.serializedObject.targetObject as Component).gameObject);
    }

    private void OnGUI()
    {
        GUIExtension.DrawGrid(Vector2.zero, new Vector2(EditorActiveAreaX, EditorActiveAreaY), 20, new(1, 0.92f, 0.016f, .2f));
        GUIExtension.DrawGrid(Vector2.zero, new Vector2(EditorActiveAreaX, EditorActiveAreaY), 100, new(1, 1, 1, .2f));
        GUIStyle extremeEngineStyle = new()
        {
            fontSize = 72,
            fontStyle = FontStyle.Bold
        };
        extremeEngineStyle.normal.textColor = new Color(.175f, .175f, .175f, 1);
        GUI.Label(new Rect(position.xMin - position.position.x, position.yMin - position.position.y, position.width, 72), ExtremeEngineData.EngineName, extremeEngineStyle);
        if (Keyboard.current.shiftKey.isPressed & Keyboard.current.aKey.isPressed)
        {
            int controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
            EditorGUIUtility.ShowObjectPicker<GameObject>(null, true, null, controlID);
        }
        /*if (commandName == "ObjectSelectorUpdated")
        {
            currentObject = EditorGUIUtility.GetObjectPickerObject();
            Repaint();
        }
        else*/
        if (Event.current.commandName == "ObjectSelectorClosed")
        {
            GameObject picked = (GameObject)EditorGUIUtility.GetObjectPickerObject();
            TryCreateNewNodeWindow(picked);
        }
        MousePosition = Event.current.mousePosition;
        if (SelectedCurve != null)
        {
            if (SelectedCurve.StartConnection != null & SelectedCurve.EndConnection == null)
            {
                ExtremeEventNode.DrawNodeCurve(SelectedCurve.StartConnection.CurveConnectPoint, MousePosition, Color.blue);
            }
            if (Mouse.current.rightButton.isPressed)
            {
                SelectedCurve = null;
            }
        }
        for (int i = 0; i < Curves.Count; i++)
        {
            Curves[i].UpdateNodeCurve();
        }
        BeginWindows();
        for (int i = 0; i < Nodes.Count; i++)
        {
            Nodes[i].UpdateWindow(i);
        }
        EndWindows();
        Repaint();
    }

    private void TryCreateNewNodeWindow(GameObject gameObject)
    {
        bool isWasCreated = false;
        for (int i = 0; i < Nodes.Count; i++)
        {
            if (Nodes[i].GameObject == gameObject)
            {
                isWasCreated = true;
                break;
            }
        }
        if (!isWasCreated)
        {
            Nodes.Add(new GameObjectNode (this, gameObject, new Vector2(position.width - MinWindowSize.x - 10, (position.height * .5f) - MinWindowSize.y), MinWindowSize, Nodes.Count));
        }
        else
        {
            Debug.Log("you cant create window from this GameObject:" + gameObject + " more then one time");
        }
    }
}
#endif