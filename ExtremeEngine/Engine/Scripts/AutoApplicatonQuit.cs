#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AutoApplicatonQuit
{
    public static void Execute()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
