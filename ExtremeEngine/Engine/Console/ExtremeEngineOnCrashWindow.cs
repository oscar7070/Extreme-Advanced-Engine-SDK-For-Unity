using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremeEngineOnCrashWindow : MonoBehaviour
{

    public void ExecuteOnCrash()
    {
        gameObject.SetActive(true);
    }

    public void ShowConsole()
    {
        gameObject.SetActive(false);
        ExtremeEngineConsoleRunner.Console.ShowConsole(true);
    }

    public void Quit()
    {
        AutoApplicatonQuit.Execute();
    }
}
