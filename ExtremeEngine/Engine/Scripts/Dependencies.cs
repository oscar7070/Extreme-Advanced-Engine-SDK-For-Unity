#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public class Dependencies : EditorWindow
{

    public static void Execute()
    {
        Dependencies window = (Dependencies)GetWindow(typeof(Dependencies), true);
        window.Show();
    }
    private enum InstalledState { NotInstalled, Installed, CheckIfInstalledCantCheckViaCode }

    private void OnGUI()
    {
        Vector2 windowSize = new(512, 192);
        maxSize = windowSize;
        minSize = windowSize;
        GUIStyle style = new();
        style.normal.textColor = Color.white;
        style.fontSize = 12;
        GUIStyle styleBold = new();
        styleBold.normal.textColor = Color.white;
        styleBold.fontStyle = FontStyle.Bold;
        styleBold.fontSize = 15;

        EditorGUI.LabelField(new Rect(10, 10, 512, 32), "If you have errors check if all dependencies installed!", styleBold);

        InstalledState unity223 = InstalledState.NotInstalled;
#if UNITY_2022_3_OR_NEWER
        unity223 = InstalledState.Installed;
#endif

        InstalledState inputSystem = InstalledState.NotInstalled;
#if ENABLE_INPUT_SYSTEM
        inputSystem = InstalledState.Installed;
#endif
        DrawInstalledOrNot(30, "Unity 2022.3 or higher", style, unity223);
        DrawInstalledOrNot(50, "InputSystem", style, inputSystem);
        DrawInstalledOrNot(70, "TextMeshPro", style, InstalledState.CheckIfInstalledCantCheckViaCode);
        DrawInstalledOrNot(90, "UnityUI", style, InstalledState.CheckIfInstalledCantCheckViaCode);
        DrawInstalledOrNot(110, "2DCommon", style, InstalledState.CheckIfInstalledCantCheckViaCode);
    }

    private void DrawInstalledOrNot(float line, string component, GUIStyle style, InstalledState installed)
    {
        switch (installed)
        {
            case InstalledState.NotInstalled:
                style.normal.textColor = Color.red;
                break;
            case InstalledState.Installed:
                style.normal.textColor = Color.green;
                break;
            case InstalledState.CheckIfInstalledCantCheckViaCode:
                style.normal.textColor = Color.gray;
                break;
        }
        EditorGUI.LabelField(new Rect(10, line, 512, 32), component + "[" + installed.ToString() + "]", style);
    }
}
#endif