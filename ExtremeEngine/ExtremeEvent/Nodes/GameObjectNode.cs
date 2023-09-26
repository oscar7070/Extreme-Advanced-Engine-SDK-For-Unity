#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class GameObjectNode : ExtremeEventNode
{

    public GameObjectNode(NodeEventEditor editor, GameObject gameObject, Vector2 position, Vector2 size, int id) : base(editor, gameObject, position, size, id)
    {

    }

    public override int OnUpdateWindow(int linePos)
    {
        const float imageSize = 20;
        linePos = MoveLine(linePos, 34);

        var allComponents = GameObject.GetComponents(typeof(Component));
        for (int i = 0; i < allComponents.Length; i++)
        {
            if (i != 0)
            {
                linePos = MoveLine(linePos);
            }
            var component = allComponents[i];
            GUI.Label(new Rect(imageSize, linePos, WindowRect.size.x - imageSize, imageSize), component.GetType().Name);
            GUIDrawSquareTextureWithCurrectResolution(new Vector2(0, linePos), EditorGUIUtility.ObjectContent(component, component.GetType()).image, imageSize);

            linePos = MoveLine(linePos);

            #region fileds
            bool fieldsFoldoutStatus = true;
            fieldsFoldoutStatus = EditorGUI.BeginFoldoutHeaderGroup(new Rect(imageSize, linePos, WindowRect.size.x - imageSize, imageSize), fieldsFoldoutStatus, new GUIContent("Properties"));
            GUIDrawSquareTextureWithCurrectResolution(new Vector2(0, linePos), Editor.PropertyIcon, imageSize);

            if (fieldsFoldoutStatus)
            {
                var componentFields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                for (int f = 0; f < componentFields.Length; f++)
                {
                    var current = componentFields[f];
                    linePos = MoveLine(linePos);
                    string arguments = "";
                    if (current.IsStatic)
                    arguments += "static ";
                    DrawConnectPoint(this, current.GetType(), linePos);
                    GUI.Label(new Rect(imageSize * 2, linePos, WindowRect.size.x - (imageSize * 2), imageSize),
                         new GUIContent(arguments + ActualClassName.ToActualName(current.FieldType.Name, false ,true) + current.Name + " = " + current.GetValue(component)?.ToString()));
                    GUIDrawSquareTextureWithCurrectResolution(new Vector2(imageSize, linePos), Editor.FieldIcon, imageSize);
                }
            }
            EditorGUI.EndFoldoutHeaderGroup();
            #endregion

            linePos = MoveLine(linePos);

            #region methods
            bool methodsFoldoutStatus = true;
            methodsFoldoutStatus = EditorGUI.BeginFoldoutHeaderGroup(new Rect(imageSize, linePos, WindowRect.size.x - imageSize, imageSize), methodsFoldoutStatus, new GUIContent("Methods"));
            GUIDrawSquareTextureWithCurrectResolution(new Vector2(0, linePos), Editor.PropertyIcon, imageSize);

            if (methodsFoldoutStatus)
            {
                var componentFields = component.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
                for (int f = 0; f < componentFields.Length; f++)
                {
                    var current = componentFields[f];
                    linePos = MoveLine(linePos);
                    string arguments = "";
                    if (current.IsStatic)
                    arguments += "static ";
                    DrawConnectPoint(this,null, linePos);
                    GUI.Label(new Rect(imageSize * 2, linePos, WindowRect.size.x - (imageSize * 2), imageSize),
                       new GUIContent(arguments + ActualClassName.ToActualName(current.ReturnParameter.ParameterType.Name) + current.Name + "(" +/* ActualClassName.ToActualName(current.()?.ToString()) +*/ ")"
                       , current.GetBaseDefinition()?.ToString()));
                    GUIDrawSquareTextureWithCurrectResolution(new Vector2(imageSize, linePos), Editor.MethodIcon, imageSize);
                }
            }
            EditorGUI.EndFoldoutHeaderGroup();
            #endregion
        }
        return linePos;
    }
}
#endif