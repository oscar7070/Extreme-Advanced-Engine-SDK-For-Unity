using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MapCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/MapCommand")]
public class MapCommand : ExtremeEngineConsoleCommand
{
    [SerializeField] private string Command = "Map";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "load a level that you want by his name";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 1)
        {
            if (DoesSceneExist(i[0]))
            {
                SceneManager.LoadScene(i[0]);
                return true;
            }
        }
        return false;
    }

    private static bool DoesSceneExist(string scene)
    {
        if (string.IsNullOrEmpty(scene))
        {
            return false;
        }
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(scene, sceneName, true) == 0)
            {
                return true;
            }
        }
        return false;
    }
}
