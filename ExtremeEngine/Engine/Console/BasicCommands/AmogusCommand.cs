using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmogusCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/AmogusCommand")]
public class AmogusCommand : ExtremeEngineConsoleCommand
{
    [SerializeField] private string Command = "Amogus";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Amogus";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 0)
        {
            Debug.Log("Did you said AMOGUS!!!");
            Debug.Log("Did you said AMOGUS!!!");
            Debug.Log("Did you said AMOGUS!!!");
            Debug.Log("EXTREME ENGINE NOW RENAMED TO INTERNATIONAL GOVERNMENT CONTROL UNIT UNDER AMOGUS GOVERNMENT");
            Debug.Log("DELETING EXTREME COMPONENTS");
            Debug.Log("DELETING EXTREME COMPONENTS 0%");
            Debug.Log("DELETING EXTREME COMPONENTS 4%");
            Debug.Log("DELETING EXTREME COMPONENTS 27%");
            Debug.Log("DELETING EXTREME COMPONENTS 70%");
            Debug.Log("DELETING EXTREME COMPONENTS 90%");
            Debug.Log("DELETING EXTREME COMPONENTS 99%");
            Debug.Log("DELETING EXTREME COMPONENTS 100%");
            Debug.Log("NULL PLUGIN IS NOT FOUND!");
            Debug.Log("AMOGUS SECURITY: CRITICAL ERROR 1x9f");
            Debug.Log("AMOGUS SECURITY: CRITICAL ERROR 42x2");
            Debug.Log("AMOGUS SECURITY: CRITICAL ERROR fx99");
            Debug.Log("YOUR HARDWARE NOT SUPPORTS <AMOGUS AGENT x64> INSTRACTIONS");
            Debug.Log("Resolving EXTREME");
            Debug.Log("EXTREME RAINSTALLED");
            Debug.Log("SUS");
            return true;
        }
        return false;
    }
}
