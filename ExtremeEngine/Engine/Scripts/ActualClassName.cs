using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualClassName
{

    public static string ToActualName(string name, bool deleteVoid = true, bool addSpaceIfNotNull = true)
    {
        switch (name)
        {
            case "Boolean":
                name = "Bool";
                break;
            case "Int32":
                name = "Int";
                break;
            case "Single":
                name = "Float";
                break;
            case "Void":
                if (deleteVoid)
                    name = null;
                break;
        }
        if (addSpaceIfNotNull && name != null)
        {
            name += " ";
        }
        return name;
    }
}
