using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PauseCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/PauseCommand")]
public class PauseCommand : ExtremeEngineConsoleCommand
{
    [SerializeField] private string Command = "Pause";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Pause or resume the game.";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 1)
        {
            switch (i[0])
            {
                case "True":
                    ExtremeEnginePause.SetPause();
                    Debug.Log("Pause changed to True!");
                    return true;
                case "False":
                    ExtremeEnginePause.SetPause(false);
                    Debug.Log("Pause changed to False!");
                    return true;
            }
        }
        return false;
    }
}