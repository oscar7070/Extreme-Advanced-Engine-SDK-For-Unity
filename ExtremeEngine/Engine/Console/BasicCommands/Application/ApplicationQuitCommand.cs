using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationQuit", menuName = ExtremeEngineData.EngineNameShort + "/Commands/ApplicationQuit")]
public class ApplicationQuitCommand : ExtremeEngineConsoleCommand
{

    [SerializeField] private string Command = "ApplicationQuit";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Quit from application.";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 0)
        {
            AutoApplicatonQuit.Execute();
            return true;
        }
        return false;
    }
}
