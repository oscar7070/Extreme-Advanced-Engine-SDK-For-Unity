#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class About : EditorWindow
{

    [SerializeField] private Texture Icon;

    [MenuItem(ExtremeEngineData.EngineName + "/About")]
    private static void AboutWindow()
    {
        About window = (About)GetWindow(typeof(About), true);
        window.Show();
    }

    private void OnGUI()
    {
        Vector2 windowSize = new(550, 148);
        maxSize = windowSize;
        minSize = windowSize;
        EditorGUI.DrawTextureTransparent(new Rect(10, 10, 128, 128), Icon);
        GUIStyle style = new();
        style.normal.textColor = Color.white;
        style.fontSize = 12;
        GUIStyle styleBold = new();
        styleBold.normal.textColor = Color.white;
        styleBold.fontStyle = FontStyle.Bold;
        styleBold.fontSize = 15;
        EditorGUI.LabelField(new Rect(148, 10, 512, 32), ExtremeEngineData.EngineName + " " + ExtremeEngineData.Version, styleBold);
        EditorGUI.LabelField(new Rect(148, 30, 512, 32), "Plugin/SDK that upgrades Unity engine capabilities.", style);
        EditorGUI.LabelField(new Rect(148, 50, 512, 32), "This software is open source.", style);
        EditorGUI.LabelField(new Rect(148, 70, 512, 32), ExtremeEngineData.ReleaseYear + " oscar7070.", style);
        if (GUI.Button(new Rect(windowSize.x - 160, windowSize.y - 35, 150, 25), "Close"))
        {
            Close();
        }
        if (GUI.Button(new Rect(windowSize.x - 320, windowSize.y - 35, 150, 25), "Dependencies"))
        {
            Dependencies.Execute();
        }
    }
}
#endif