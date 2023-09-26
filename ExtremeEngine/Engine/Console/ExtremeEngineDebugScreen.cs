using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremeEngineDebugScreen : MonoBehaviour
{

    [SerializeField] private ExtremeEngineConsole Console;
    [SerializeField] private ExtremeEngineOnCrashWindow OnCrashWindow;
    [SerializeField] private StaticCoroutineRunner GlobalMonoCoroutine;

    public ExtremeEngineConsole GetConsole()
    {
        return Console;
    }

    public ExtremeEngineOnCrashWindow GetCrashWindow()
    {
        return OnCrashWindow;
    }

    public StaticCoroutineRunner GetGlobalMonoCoroutine()
    {
        return GlobalMonoCoroutine;
    }
}
