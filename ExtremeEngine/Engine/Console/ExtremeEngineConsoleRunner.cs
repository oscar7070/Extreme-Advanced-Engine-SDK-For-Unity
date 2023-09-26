using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ExtremeEngineConsoleRunner
{

    private static bool GameCrashed = false;
    private static string ConsoleText = null;
    public static ExtremeEngineDebugScreen DebugScreen = null;
    public static ExtremeEngineConsole Console = null;
    public static ExtremeEngineConsoleProperties Properties = null;
    public delegate void ConsoleTextReloadedDelegate(string text);
    public static ConsoleTextReloadedDelegate ConsoleTextReloaded;

    //console prefab name to find
    private const string UsedConsolePropertiesName = "USEDEEConsolePrefabProperties000x01";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InalizeOnLoadMethed()
    {
        Debug.Log(ExtremeEngineData.EngineName + " starting...");
        DebugScreen = Instance;
        Console = DebugScreen.GetConsole();
    }

    private static ExtremeEngineDebugScreen Instance
    {
        get
        {
            Application.logMessageReceived += OnLog;
            if (!DebugScreen)
            {
                Properties = Resources.Load<ExtremeEngineConsoleProperties>(UsedConsolePropertiesName);
                if (Properties == null)
                {
                    Debug.LogError("Can't run " + ExtremeEngineData.EngineName + " console properties with this name not found: " + UsedConsolePropertiesName + "it's needs to be in a resources folder to dont be deleted on compile");
                }
                else
                {
                    DebugScreen = Properties.Execute();
                    if (DebugScreen != null)
                    {
                        UnityEngine.Object.DontDestroyOnLoad(DebugScreen);
                    }
                }
            }
            return DebugScreen;
        }
    }

    public static bool GetIfCrashed()
    {
        return GameCrashed;
    }

    private static IEnumerator ExecuteCrash(AsyncOperation[] scenes)
    {
        if (CheckIfAllAsyncOperationsComplete(scenes))
        {
            yield return null;
        }
        ExecuteCrashWindow();
    }

    private static bool CheckIfAllAsyncOperationsComplete(AsyncOperation[] operations)
    {
        for (int i = 0; i < operations.Length; i++)
        {
            if (!operations[i].isDone)
            {
                return false;
            }
        }
        return true;
    }

    public static void ExecuteCrashWindow()
    {
        Console.HideConsole();
        GameCrashed = true;
        Console.SetCrashCursor();
        DebugScreen.GetCrashWindow().ExecuteOnCrash();
        Camera camera = new GameObject().AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.Color;
        camera.backgroundColor = Color.black;
    }

    private static void OnLog(string log, string stackTrace, LogType type)
    {
        string finalText = "<i>" + LogTypeToIcon(type) + "</i>" + log;// + "Trace: " + stackTrace;
        bool error = false;
        switch (type)
        {
            case LogType.Error or LogType.Exception or LogType.Assert:
                finalText = "<color=red><line-height=75%>" + finalText + "Trace: " + stackTrace + "</color><br><br>";
                error = true;
                break;
            case LogType.Warning:
                finalText = "<color=yellow><line-height=75%>" + finalText + "Trace: " + stackTrace + "</color><br><br>";
                break;
            case LogType.Log:
                finalText = "<color=white><line-height=75%>" + finalText + "</color><br><br>";
                break;
        }
        ConsoleText += finalText;

        ConsoleTextReloaded?.Invoke(ConsoleText);
        if (!GameCrashed && (error & Properties.CheckIfCanCrash()))
        {
            AsyncOperation[] scenesUnloadProgress = new AsyncOperation[SceneManager.sceneCount];
            var newScene = SceneManager.CreateScene("[" + ExtremeEngineData.EngineNameShort + "]Error0000x01");
            for (int i = 0; i < scenesUnloadProgress.Length; i++)
            {
                scenesUnloadProgress[i] = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }
            SceneManager.SetActiveScene(newScene);
            StaticCoroutineRunner.StartStaticCoroutine(ExecuteCrash(scenesUnloadProgress));
        }
    }

    private static string LogTypeToIcon(LogType type)
    {
        string IconToDisplay = type.ToString();
        switch (type)
        {
            case LogType.Error:
                IconToDisplay = "<sprite name=\"ErrorIcon\">";
                break;
            case LogType.Exception or LogType.Assert:
                IconToDisplay = "<sprite name=\"ErrorIcon\">" + IconToDisplay;
                break;
            case LogType.Warning:
                IconToDisplay = "<sprite name=\"WarningIcon\">";
                break;
            case LogType.Log:
                IconToDisplay = "<sprite name=\"LogIcon\">";
                break;
        }
        return IconToDisplay;
    }

    public static string GetConsoleText()
    {
        return ConsoleText;
    }

    public static void Clear()
    {
        ConsoleText = null;
        ConsoleTextReloaded?.Invoke(ConsoleText);
    }
}
