using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtremeEngineDebugExt
{

    public static void LogArray(string header, string[] elements)
    {
        string log = "<color=green><u><b>" + header + "</b></u><br><br>";
        for (int i = 0; i < elements.Length; i++)
        {
            log += "-<color=green>" + elements[i];
            if (i != elements.Length - 1)
            {
                log += "\n";
            }
        }
        Debug.Log(log);
    }
}
