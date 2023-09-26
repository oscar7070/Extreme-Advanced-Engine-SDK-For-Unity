using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;

[Serializable]
public class NodeEvent //: UnityEvent
{

    public class PairTo
    {
        public object PropertyFrom;
        public List<object> PropertyTo;
    }

    public List<object> Values;
    public List<PairTo> PairedTo;

    public NodeEvent(List<object> values)
    {
        Values = values;
    }
}