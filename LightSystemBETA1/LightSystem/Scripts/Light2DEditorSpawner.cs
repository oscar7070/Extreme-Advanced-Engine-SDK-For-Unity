#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class Light2DEditorSpawner : MonoBehaviour
{

    private const string Path = "LightSystem2D/";

    [MenuItem(Path + "GlobalLight2D")]
    private static void CreateGlobalLight2D(MenuCommand menuCommand)
    {
        ObjectSpawner(new GameObject("GlobalLight2D").AddComponent<GlobalLight2D>().gameObject, menuCommand);
    }

    [MenuItem(Path + "Light2D")]
    private static void CreateLight2D(MenuCommand menuCommand)
    {
        ObjectSpawner(new GameObject("Light2D").AddComponent<Light2D>().gameObject, menuCommand);
    }

    [MenuItem(Path + "Light2DCollider")]
    private static void CreateLight2DCollider(MenuCommand menuCommand)
    {
        ObjectSpawner(new GameObject("Light2DCollider").AddComponent<Light2DCollider>().gameObject, menuCommand);
    }

    [MenuItem(Path + "Light2DSceneData")]
    private static void CreateLight2DSceneData(MenuCommand menuCommand)
    {
        ObjectSpawner(new GameObject("Light2DSceneData").AddComponent<Light2DSceneData>().gameObject, menuCommand);
    }

    private static GameObject ObjectSpawner(GameObject obj, MenuCommand menuCommand)
    {
        Vector2 posToSet = SceneView.lastActiveSceneView.camera.transform.position;
        obj.transform.position = posToSet;
        GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
        Selection.activeObject = obj;
        return obj;
    }
}
#endif