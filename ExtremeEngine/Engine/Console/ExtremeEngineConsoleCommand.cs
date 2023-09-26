using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExtremeEngineConsoleCommand : ScriptableObject
{
    public virtual string ICommand { get; }
    public virtual string IDescription { get; }

    //propertiesCollection[i] not contains the command name its self
    //not like <map level1>
    //like <level1>
    public virtual bool OnCommandExecute(string[] propertiesCollection)
    {
        return true;
    }
}