using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/InfoCommand")]
public class InfoCommand : ExtremeEngineConsoleCommand
{

    [SerializeField] private string Command = "Info";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Some information that can be helpful";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        if (i.Length == 0)
        {
            string[] str =
            {
            "Product name: " + Application.productName,
            "Product version: " + Application.version ,
            "Unity version: " + Application.unityVersion,
            ExtremeEngineData.EngineName + " based on " + ExtremeEngineData.BasedOn + " version: " + ExtremeEngineData.Version,
            "API: " + SystemInfo.graphicsDeviceType ,
            "CPU: " + SystemInfo.processorType,
            "CPU cores count: " + SystemInfo.processorCount ,
            "GPU: " + SystemInfo.graphicsDeviceName ,
            "GPU vendor: " + SystemInfo.graphicsDeviceVendor,
            "GPU memory: " + SystemInfo.graphicsMemorySize,
            "Memory: " + SystemInfo.systemMemorySize,
            "Operating system: " + SystemInfo.operatingSystem,
            "Operating system family: " + SystemInfo.operatingSystemFamily,
            "Platform: " + Application.platform ,
            "Target frame rate: " + Application.targetFrameRate,
            "System language: " + Application.systemLanguage
            };
            ExtremeEngineDebugExt.LogArray("Info below:", str);
            return true;
        }
        return false;
    }
}
