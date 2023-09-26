#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExtremeEventNode
{

    public readonly NodeEventEditor Editor;
    public GameObject GameObject;
    public SerializedProperty From;
    public Rect WindowRect;
    public List<ConnectPoint> ConnectPoints = new();
    private Vector2 WindowSize = new(400, 100);

    public class ConnectPoint
    {
        public ExtremeEventNode Window;
        public NodeCurve ConnectedTo;
        public object Object;
        public Vector2 CurveConnectPoint;

        public ConnectPoint(object obj, ExtremeEventNode window)
        {
            Object = obj;
            Window = window;
        }

        public void Update(int id, NodeEventEditor editor)
        {
            const float pointSize = 22;
            if (ConnectedTo == null)
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.red;
            }
            GUIStyle circleButtonStyle = new();
            Vector2 boxPosition;
            if (id == 0)
            {
                boxPosition = new(Window.WindowRect.x - pointSize, Window.WindowRect.y);
                CurveConnectPoint = boxPosition + new Vector2(0, pointSize * .5f);
                circleButtonStyle.normal.background = (Texture2D)editor.ConnectPointInButtonIcon;
            }
            else
            {
                boxPosition = new(Window.WindowRect.x + Window.WindowRect.size.x, Window.WindowRect.y);
                CurveConnectPoint = boxPosition + new Vector2(pointSize, pointSize * .5f);
                circleButtonStyle.normal.background = (Texture2D)editor.ConnectPointOutButtonIcon;
            }
            var button = GUI.Button(new Rect(boxPosition, new(pointSize, pointSize)), new GUIContent(), circleButtonStyle);
            if (button & ConnectedTo == null)
            {
                if (editor.SelectedCurve == null)
                {
                    editor.SelectedCurve = new NodeCurve(this, null);
                }
                else
                {
                    editor.SelectedCurve.Connect(this, editor);
                }
            }
            else if (button & ConnectedTo != null)
            {
                ConnectedTo.Destroy(editor);
            }
        }
    }

    public int GetID(ExtremeEventNode node)
    {
        for (int i = 0; i < Editor.Nodes.Count; i++)
        {
            if (Editor.Nodes[i] == node)
            {
                return i;
            }
        }
        return -1;
    }

    protected void DrawConnectPoint(ExtremeEventNode node, Type type, float verticalPosition, float size = 10)
    {
        Handles.BeginGUI();

        float halfSize = size * .5f;
        Vector2 toAdd = new(halfSize, halfSize);
        Handles.color = Color.white;
        float horizontalPosition = 0;
        if (node.GetID(node) != 0)
        {
            horizontalPosition = node.WindowRect.width - (size * 2);
        }
        GUI.Box(new Rect(0, verticalPosition + (halfSize  *.5f), node.WindowRect.width, size * 1.5f), "");
        Handles.DrawSolidRectangleWithOutline(new Rect(horizontalPosition + toAdd.x, verticalPosition + toAdd.y, size, size), new Color(.2f, .2f, .2f, 1), Color.green);
        //Handles.color = new Color(.5f, .5f, .5f, 1);
        //.DrawSolidDisc(position, Vector3.forward, halfSize * .8f);
        Handles.EndGUI();
    }

    public ExtremeEventNode(NodeEventEditor editor, GameObject gameObject, Vector2 position, Vector2 size, int id)
    {
        Editor = editor;
        GameObject = gameObject;
        WindowRect = new(position, size);
        //ConnectPoints.Add(new(null, this));
    }

    public void UpdateWindow(int id)
    {
        if (id == 0)
        {
            GUI.color = new Color(.75f, 1, .75f, 1);
        }
        else
        {
            GUI.color = Color.white;
        }
        GUIStyle windowStyle = GUI.skin.window;
        WindowRect = GUI.Window(id, new(WindowRect.position, WindowSize), DrawNodeWindow, "", windowStyle);
        for (int i = 0; i < ConnectPoints.Count; i++)
        {
            ConnectPoints[i].Update(id, Editor);
        }
    }

    public void DrawNodeWindow(int id)
    {
        GUIExtension.DrawGrid(Vector2.zero, new Vector2(WindowRect.size.x, WindowRect.size.y), 20, new(.7f, .7f, 1, .1f));
        const float headerBoxSize = 32;
        const float closeButtonSize = 32;
        int linePos = 0;

        GUI.backgroundColor = new Color(.7f, .7f, 1, 1);
        GUI.contentColor = new(1, .5f, .5f, 1);

        GUI.Box(new Rect(0, linePos, WindowRect.size.x, headerBoxSize), new GUIContent());
        GUIDrawSquareTextureWithCurrectResolution(new Vector2(0, linePos), EditorGUIUtility.ObjectContent(GameObject, GameObject.GetType()).image, headerBoxSize);

        GUIStyle headerText = new()
        {
            alignment = TextAnchor.MiddleLeft,
            fontSize = 16
        };
        GUI.Label(new Rect(headerBoxSize, linePos, WindowRect.size.x - headerBoxSize - closeButtonSize, headerBoxSize), GameObject.name, headerText);

        if (id != 0)
        {
            GUIStyle closeButtonStyle = new();
            closeButtonStyle.normal.background = (Texture2D)Editor.CloseButtonIcon;
            if (GUI.Button(new Rect(new(WindowRect.size.x - closeButtonSize, 0), new(closeButtonSize, closeButtonSize)), new GUIContent(), closeButtonStyle))
            {
                Destroy();
            }
        }

        linePos = OnUpdateWindow(linePos);

        WindowSize = new(WindowSize.x, linePos);
        GUI.DragWindow();
    }

    public virtual int OnUpdateWindow(int linePos)
    {
        return linePos;
    }

    protected static void GUIDrawSquareTextureWithCurrectResolution(Vector2 position, Texture image, float imageSize)
    {
        GUI.DrawTexture(new Rect(position, new Vector2(imageSize, imageSize)), image, ScaleMode.ScaleToFit);
    }

    protected static int MoveLine(int CurrentLinePos, int ToAdd = 22)
    {
        return CurrentLinePos + ToAdd;
    }

    private void Destroy()
    {
        Editor.Nodes.Remove(this);
    }

    public class NodeCurve
    {

        public ConnectPoint StartConnection { get; private set; }
        public ConnectPoint EndConnection { get; private set; }

        public NodeCurve(ConnectPoint startConnection, ConnectPoint endConnection)
        {
            StartConnection = startConnection;
            EndConnection = endConnection;
        }

        public void Connect(ConnectPoint point, NodeEventEditor editor)
        {
            EndConnection = point;
            if (StartConnection != null)
            {
                OnConnectedFromBothSides(editor);
            }
        }

        private void OnConnectedFromBothSides(NodeEventEditor editor)
        {
            editor.Curves.Add(this);
            editor.SelectedCurve = null;
            StartConnection.ConnectedTo = this;
            EndConnection.ConnectedTo = this;
        }

        public void Destroy(NodeEventEditor editor)
        {
            if (editor.Curves.Contains(this))
            {
                StartConnection.ConnectedTo = null;
                EndConnection.ConnectedTo = null;
                editor.Curves.Remove(this);
            }
            else if (editor.SelectedCurve == this)
            {
                editor.SelectedCurve = null;
            }
        }

        public void UpdateNodeCurve()
        {
            DrawNodeCurve(StartConnection.CurveConnectPoint, EndConnection.CurveConnectPoint, Color.green);
        }
    }

    public static void DrawNodeCurve(Vector2 start, Vector2 end, Color color)
    {
        const float bezierIntensity = 50;
        Vector2 startTan = start + Vector2.right * bezierIntensity;
        Vector2 endTan = end + Vector2.left * bezierIntensity;
        Handles.DrawBezier(start, end, startTan, endTan, color, null, 5);
    }
}
#endif