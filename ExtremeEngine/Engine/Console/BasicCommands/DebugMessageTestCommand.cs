using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugMessageTestCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/DebugMessageTestCommand")]
public class DebugMessageTestCommand : ExtremeEngineConsoleCommand
{
    [SerializeField] private string Command = "DebugMessageTest";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Displays selected message type such as error to see the reaction on it.";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 1)
        {
            switch (i[0])
            {
                case "Error":
                    Debug.LogError("ErrorTest");
                    return true;
                case "Assert":
                    Debug.LogAssertion("AssertTest");
                    return true;
                case "Warning":
                    Debug.LogWarning("WarningTest");
                    return true;
                case "Log":
                    Debug.Log("LogTest");
                    return true;
                case "Exception":
                    Debug.LogException(new System.Exception("ExceptionTest"));
                    return true;
            }
        }
        return false;
    }
}
