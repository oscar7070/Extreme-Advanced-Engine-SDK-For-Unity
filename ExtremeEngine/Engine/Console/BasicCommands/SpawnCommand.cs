using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/SpawnCommand")]
public class SpawnCommand : ExtremeEngineConsoleCommand
{
    [SerializeField] private string Command = "Spawn";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Spawn a GameObject that you want";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 1)
        {
            //if (Resources.Load(i[0]) != null)
            //{
                return true;
            //}
        }
        return false;
    }
}