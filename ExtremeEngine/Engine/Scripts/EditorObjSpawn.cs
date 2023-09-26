#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorObjSpawn
{

    public static GameObject Invoke(GameObject obj/*, MenuCommand menuCommand*/)
    {
        Vector2 posToSet = SceneView.lastActiveSceneView.camera.transform.position;
        obj.transform.position = posToSet;
        //GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        Selection.activeObject = obj;
        return obj;
    }
}
#endif