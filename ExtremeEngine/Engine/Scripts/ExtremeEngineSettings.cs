#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public class ExtremeEngineSettings : EditorWindow
{

    [SerializeField] private Texture Icon;
    [SerializeField] private ExtremeEngineData Data;// => AssetDatabase.getAssets / Plugins/ExtremeEngine/Engine/ExtremeEngineDataProperties.asset;

    [MenuItem(ExtremeEngineData.EngineName + "/Settings")]
    private static void SettingsWindow()
    {
        ExtremeEngineSettings window = (ExtremeEngineSettings)GetWindow(typeof(ExtremeEngineSettings));
        window.Show();
    }

    private void OnGUI()
    {
        /*if (Data == null)
        {
            //Data = AssetDatabase.CreateAsset(CreateInstance(typeof(ExtremeEngineData)), "Assets/Plugins/ExtremeEngine/Engine/ExtremeEngineDataProperties.asset");
            AssetDatabase.SaveAssets();
        }
        GUIStyle styleHeader = new();
        styleHeader.normal.textColor = Color.white;
        styleHeader.fontStyle = FontStyle.Bold;
        styleHeader.fontSize = 20;
        //EditorGUI.DrawTextureTransparent(new Rect(10, 10, 128, 128), Icon);
        EditorGUILayout.LabelField("Settings", styleHeader);

        EditorGUI.BeginChangeCheck();
        var newLight2DSelectedSettings = (Light2DSettings)EditorGUILayout.ObjectField("Light system2D settings", Data.Properties.Light2DSelectedSettings, typeof(Light2DSettings), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(newLight2DSelectedSettings, "Light2DSettingsFileSelection");
            Data.Properties.Light2DSelectedSettings = newLight2DSelectedSettings;
        }
        //EditorGUILayout.PropertyField(Light2DSelectedSettings);

        EditorGUI.BeginChangeCheck();
        var newExtremeEngineConsoleProperties = (ExtremeEngineConsoleProperties)EditorGUILayout.ObjectField("Extreme engine console settings", Data.Properties.ExtremeEngineConsoleSelectedProperties, typeof(ExtremeEngineConsoleProperties), false);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(newExtremeEngineConsoleProperties, "ExtremeEngineConsoleFileSelection");
            Data.Properties.ExtremeEngineConsoleSelectedProperties = newExtremeEngineConsoleProperties;
        }*/
    }

    /*private void ObjectFieldWithUndo(Type objectType, string description, object saveTo, bool allowObjectsFromScene = false)
    {
        EditorGUI.BeginChangeCheck();
        var newObj = (objectType)EditorGUILayout.ObjectField(description, saveTo, typeof(objectType), allowObjectsFromScene);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(newObj, "ExtremeEngineSettings property Changed");
            saveTo = newObj;
        }
    }*/
}
#endif

public class ExtremeEngineData
{

    private static string EngineFolderPath;
    //public Texture ExtremeEngineLogo;
    public static string GetEngineFolderPath => EngineFolderPath;

    //if it's a modification on Extreme Engine you can change names but leave the basedOn value.
    public const string EngineName = "Extreme Advanced Engine";
    public const string EngineNameShort = "Extreme Engine";


    //Please do not change based on 'Extreme Advanced Engine SDK'.
    public const string BasedOn = "Extreme Advanced Engine SDK by: oscar7070";

    public const string Version = "1.0";
    public const string ReleaseYear = "2023";
    //public PropertiesClass Properties;

    /*[Serializable]
    public class PropertiesClass
    {
        public Light2DSettings Light2DSelectedSettings;
        public ExtremeEngineConsoleProperties ExtremeEngineConsoleSelectedProperties;
    }*/
}