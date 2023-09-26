using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class GlobalEventSystem
{

    private static InputSystemUIInputModule InputModule;
    private static EventSystem EventSystem;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InalizeOnLoadMethed()
    {
        Debug.Log("Creating " + ExtremeEngineData.EngineName + " GlobalEventSystem");
        InputModule = Instance;
        EventSystem = InputModule.GetComponent<EventSystem>();
    }

    private static InputSystemUIInputModule Instance
    {
        get
        {
            if (!InputModule)
            {
                InputSystemUIInputModule inputModuleObj = new GameObject("GlobalEventSystem").AddComponent<InputSystemUIInputModule>();
                Object.DontDestroyOnLoad(inputModuleObj.gameObject);
                return inputModuleObj;
            }
            return InputModule;
        }
    }

    public static InputSystemUIInputModule GetInputModule()
    {
        return InputModule;
    }

    public static EventSystem GetEventSystem()
    {
        return EventSystem;
    }
}
