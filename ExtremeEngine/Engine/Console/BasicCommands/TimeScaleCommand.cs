using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeScaleCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/TimeScaleCommand")]
public class TimeScaleCommand : ExtremeEngineConsoleCommand
{

    [SerializeField] private string Command = "TimeScale";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Command to change application playback speed.(TimeScale <PlaySpeed>).";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 1)
        {
            if (float.TryParse(i[0], out float time))
            {
                Time.timeScale = time;
            }
            else
            {
                Debug.Log("Please write the command currently[TimeScale <PlaySpeed>]");
            }
            return true;
        }
        return false;
    }
}
