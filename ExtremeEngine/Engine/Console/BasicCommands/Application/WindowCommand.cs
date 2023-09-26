using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WindowCommand", menuName = ExtremeEngineData.EngineNameShort + "/Commands/WindowCommand")]
public class WindowCommand : ExtremeEngineConsoleCommand
{

    [SerializeField] private string Command = "Window";
    public override string ICommand => Command;

    [SerializeField, TextArea] private string Description = "Command to changes application window settings(Size <X> <Y>, MaxFPS <FPS>, Windowed, MaximizedWindow, FullScreen, ExclusiveFullScreen).";
    public override string IDescription => Description;

    public override bool OnCommandExecute(string[] i)
    {
        #region command
        //Window size X Y
        //window windowed
        //window MaximizedWindow
        //Window fullscreen
        //window exclusivefullscreen
        //window maxfps
        #endregion
        switch (i.Length)
        {
            case 3:
                if (i[0] == "Size")
                {
                    if (int.TryParse(i[1], out int x) && int.TryParse(i[2], out int y))
                    {
                        Screen.SetResolution(x, y, Screen.fullScreenMode);
                    }
                    else
                    {
                        Debug.Log("Please write screen resolution[Window Size <X> <Y>]");
                    }
                    return true;
                }
                break;
            case 2:
                if (i[0] == "MaxFPS")
                {
                    if (int.TryParse(i[1], out int maxFPS))
                    {
                        Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, maxFPS);
                    }
                    else
                    {
                        Debug.Log("Please write Max frame rate[Window MaxFPS <FPS>]");
                    }
                    return true;
                }
                break;
            case 1:
                switch (i[0])
                {
                    case "Windowed":
                        Screen.fullScreenMode = FullScreenMode.Windowed;
                        return true;
                    case "MaximizedWindow":
                        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                        return true;
                    case "FullScreen":
                        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                        return true;
                    case "ExclusiveFullScreen":
                        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                        return true;
                }
                break;
        }
        return false;
    }
}
