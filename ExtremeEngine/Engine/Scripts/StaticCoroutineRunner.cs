using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class StaticCoroutineRunner : MonoBehaviour
{

    private static StaticCoroutineRunner CoroutineRunner;
    private static int NumCoroutinesRunning;

    public static Coroutine StartStaticCoroutine(IEnumerator coroutine)
    {
        NumCoroutinesRunning++;
        if (CoroutineRunner == null)
        {
            CoroutineRunner = new GameObject("CoroutineRunner").AddComponent<StaticCoroutineRunner>();
        }

        return CoroutineRunner.StartCoroutine(coroutine);
    }

    public static void StopStaticCoroutine(IEnumerator coroutine)
    {
        if (CoroutineRunner != null)
        {
            CoroutineRunner.StopCoroutine(coroutine);
        }
    }

    public static void CleanUpRunnerIfDone()
    {
        NumCoroutinesRunning--;
        if (NumCoroutinesRunning == 0 && CoroutineRunner != null)
        {
            DestroyImmediate(CoroutineRunner.gameObject);
        }
    }
}