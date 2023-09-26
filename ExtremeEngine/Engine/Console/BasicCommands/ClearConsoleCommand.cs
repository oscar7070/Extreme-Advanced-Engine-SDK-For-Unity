using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClearConsoleCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/ClearConsoleCommand")]
public class ClearConsoleCommand : ExtremeEngineConsoleCommand
{
    [SerializeField] private string Command = "ClearConsole";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Clears all console text.";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 0)
        {
            ExtremeEngineConsoleRunner.Clear();
            return true;
        }
        return false;
    }
}
