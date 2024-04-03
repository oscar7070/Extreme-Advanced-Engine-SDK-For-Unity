using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public static class Light2DSceneData
{

    private static List<Light2DCollider> Light2DColliders = new();

    public static void AddCollider(Light2DCollider toAdd)
    {
        if (!Light2DColliders.Contains(toAdd))
        {
            Light2DColliders.Add(toAdd);
        }
    }

    public static void RemoveCollider(Light2DCollider toRemove)
    {
        //if (toRemove != null && Light2DColliders.Contains(toRemove))
        //{
            Light2DColliders.Remove(toRemove);
        //}
    }

    public static List<Light2DCollider> GetAllColliders()
    {
        return Light2DColliders;
    }
}
