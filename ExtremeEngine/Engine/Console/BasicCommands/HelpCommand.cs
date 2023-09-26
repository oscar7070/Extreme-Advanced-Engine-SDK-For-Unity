using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Help", menuName = ExtremeEngineData.EngineNameShort + "/Commands/Help")]
public class HelpCommand : ExtremeEngineConsoleCommand
{

    [SerializeField] private string Command = "Help";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Help command.";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 0)
        {
            GetHelpAboutCommand.WriteAboutAllCommands();
            Debug.Log("There is all commands given to you that will help you!");
            Debug.Log("Use any command on your RISK!");
            return true;
        }
        return false;
    }
}
