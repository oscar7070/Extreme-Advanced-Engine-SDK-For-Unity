using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtremePostAsset", menuName = ExtremeEngineData.EngineNameShort + "/ExtremePostAsset")]
public class ExtremePostAsset : ScriptableObject
{

    [SerializeField] private string Name = "Low";
    [Header("Set the post effects in true order.")]
    [SerializeField] private ExtremePost[] PostsEffects;

}
