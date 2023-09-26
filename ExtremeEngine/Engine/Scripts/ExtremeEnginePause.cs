using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtremeEnginePause
{

    public static bool IsPaused { get; private set; } = false;

    public delegate void OnPauseDelegate();
    public static OnPauseDelegate OnPause;

    public delegate void OnResumeDelegate();
    public static OnResumeDelegate OnResume;

    public static void SetPause(bool pause = true)
    {
        if (pause)
        {
            IsPaused = true;
            OnPause?.Invoke();
            //AudioListener.pause = true;
            Time.timeScale = 0;
        }
        else
        {
            IsPaused = false;
            OnResume?.Invoke();
            //AudioListener.pause = false;
            Time.timeScale = 0;
        }
    }
}
