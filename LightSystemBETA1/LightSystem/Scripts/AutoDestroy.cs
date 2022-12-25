using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    public static void Execute(Object obj)
    {
#if UNITY_EDITOR
        DestroyImmediate(obj);
#else
        Destroy(obj);
#endif
    }
}
