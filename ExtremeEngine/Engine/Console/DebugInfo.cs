using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class DebugInfo : MonoBehaviour
{

    private TextMeshPro Text;
    [SerializeField] private DebugInfoUnit[] Units;


    private void Start()
    {
        Text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        for (int i = 0; i < Units.Length; i++)
        {
            Units[i].OnDisplay();
        }
    }
}

[Serializable]
public class DebugInfoUnit
{

    public virtual string OnDisplay()
    {
        string str = "";
        return str;
    }
}

public class Usage : DebugInfoUnit
{

    public override string OnDisplay()
    {
        string str = "";
        //CPU
        str = SystemInfo.processorType.ToString();
        return str;
    }
}