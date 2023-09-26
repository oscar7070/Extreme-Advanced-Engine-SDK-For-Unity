#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NodeEvent))]
public class NodeEventCustomInspector : PropertyDrawer
{

    public Texture2D Icon;
    public Texture2D IconGreen;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.DrawRect(position, new Color(0,1,0,1 * .2f));
        EditorGUI.LabelField(new Rect(0, position.y, position.width * .25f, position.height), new GUIContent("ExtremeEvent"));
        if (GUI.Button(new Rect(position.width * .25f, position.y, position.width * .75f, position.height), new GUIContent("Open node editor(" + label + "<" /*+ new PropertyField(property.FindPropertyRelative("Value"))*/ + ">" + ")")))
        {
            NodeEventEditor.Create().Run(property);
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return EditorGUI.GetPropertyHeight(property) + 20;
        }
        return EditorGUI.GetPropertyHeight(property);
    }

    public System.Object GetPropertyInstance(SerializedProperty property)
    {

        string path = property.propertyPath;

        System.Object obj = property.serializedObject.targetObject;
        var type = obj.GetType();

        var fieldNames = path.Split('.');
        for (int i = 0; i < fieldNames.Length; i++)
        {
            var info = type.GetField(fieldNames[i]);
            if (info == null)
                break;

            // Recurse down to the next nested object.
            obj = info.GetValue(obj);
            type = info.FieldType;
        }

        return obj;
    }
}
#endif