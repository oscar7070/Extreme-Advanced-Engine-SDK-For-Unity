using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Light2DSceneData : MonoBehaviour
{

    private readonly List<Light2DCollider> Light2DColliders = new();

    private void Start()
    {
        Light2DSceneData[] SceneDatas = FindObjectsOfType<Light2DSceneData>();
        for (int i = 0; i < SceneDatas.Length; i++)
        {
            if (SceneDatas[i] != this)
            {
                if (!Application.isPlaying)
                {
                    DestroyImmediate(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
                Debug.LogWarning("You can't add more then one Light2DSceneData object");
            }
        }
    }

    public void AddCollider(Light2DCollider toAdd)
    {
        if (!Light2DColliders.Contains(toAdd))
        {
            Light2DColliders.Add(toAdd);
        }
    }

    public void RemoveCollider(Light2DCollider toRemove)
    {
        //if (toRemove != null && Light2DColliders.Contains(toRemove))
        //{
            Light2DColliders.Remove(toRemove);
        //}
    }

    public List<Light2DCollider> GetAllColliders()
    {
        return Light2DColliders;
    }

    private void OnDestroy()
    {
        Light2DColliders.Clear();
    }
}
