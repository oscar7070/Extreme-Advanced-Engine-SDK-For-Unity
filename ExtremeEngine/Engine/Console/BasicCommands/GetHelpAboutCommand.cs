using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelpAbout", menuName = ExtremeEngineData.EngineNameShort + "/Commands/HelpAbout")]
public class GetHelpAboutCommand : ExtremeEngineConsoleCommand
{
    [SerializeField] private string Command = "HelpAbout";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Command that shows what other commands does";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 1)
        {
            if (i[0] == "AllCommands")
            {
                WriteAboutAllCommands();
                return true;
            }
        }
        else if (i.Length == 2)
        {
            if (i[0] == "Command")
            {
                var allCommands = ExtremeEngineConsoleRunner.Properties.Commands;
                for (int p = 0; p < allCommands.Length; p++)
                {
                    if (i[1] == allCommands[p].Command.ICommand)
                    {
                        WriteAboutCommand(allCommands[p]);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static void WriteAboutAllCommands()
    {
        var allCommands = ExtremeEngineConsoleRunner.Properties.Commands;
        for (int p = 0; p < allCommands.Length; p++)
        {
            WriteAboutCommand(allCommands[p]);
        }
    }

    private static void WriteAboutCommand(ExtremeEngineConsoleProperties.CommandProperties commandProperties)
    {
        var console = ExtremeEngineConsoleRunner.Console;
        if (console.GetProperties().CheckIfCanUseCommandPermission(commandProperties.Permissions, console.GetCheatsEnabled()))
        {
            Debug.Log("<" + commandProperties.Command.ICommand + "> " + commandProperties.Command.IDescription);
        }
        else if (commandProperties.Permissions == ExtremeEngineConsoleProperties.CommandProperties.CommandTypes.EnableInBuildWithCheatsOn)
        {
            Debug.Log("<color=orange><" + commandProperties.Command.ICommand + "> " + commandProperties.Command.IDescription + "</color><color=red>{Requires enabled cheats permission}</color>");
        }
    }
}
