using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheatsCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/CheatsCommand")]
public class ExtremeEngineCheatsCommand : ExtremeEngineConsoleCommand
{
    [SerializeField] private string Command = "CheatsEnabled";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Set's the cheats commands enabled.";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 1)
        {
            if (i[0] == "True")
            {
                Debug.Log("Cheats enabled to true");
                ExtremeEngineConsoleRunner.Console.SetCheatsEnabled(true);
                return true;
            }
            else if (i[0] == "False")
            {
                Debug.Log("Cheats enabled to false");
                ExtremeEngineConsoleRunner.Console.SetCheatsEnabled(false);
                return true;
            }
        }
        return false;
    }
}
