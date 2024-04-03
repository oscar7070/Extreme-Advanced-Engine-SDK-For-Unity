#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class Light2DEditorSpawner
{

    private const string Path = ExtremeEngineData.EngineName + "/LightSystem2D/";

    [MenuItem(Path + "GlobalLight2D")]
    private static void CreateGlobalLight2D(MenuCommand menuCommand)
    {
        EditorObjSpawn.Invoke(new GameObject("GlobalLight2D").AddComponent<GlobalLight2D>().gameObject);
    }

    [MenuItem(Path + "Light2D")]
    private static void CreateLight2D(MenuCommand menuCommand)
    {
        EditorObjSpawn.Invoke(new GameObject("Light2D", typeof(Light2D)));
    }

    [MenuItem(Path + "Light2DCollider")]
    private static void CreateLight2DCollider(MenuCommand menuCommand)
    {
        EditorObjSpawn.Invoke(new GameObject("Light2DCollider").AddComponent<Light2DCollider>().gameObject);
    }
}
#endif