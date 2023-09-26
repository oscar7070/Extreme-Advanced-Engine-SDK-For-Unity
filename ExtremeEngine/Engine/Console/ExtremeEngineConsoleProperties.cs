using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsoleProperties", menuName = ExtremeEngineData.EngineNameShort + "/ConsoleProperties")]
public class ExtremeEngineConsoleProperties : ScriptableObject
{

    [SerializeField] private int MaximumSavedLogLines = 200;
    [SerializeField] private ExtremeEngineDebugScreen DebugScreen;
    public enum ConsolePermissionsTypes { Disable, OnlyEditor, EnableInDevolomentBuild, EnableInBuild }
    [SerializeField] private ConsolePermissionsTypes ConsolePermissions = ConsolePermissionsTypes.EnableInBuild;
    [SerializeField] private bool EnableGameCrashInEditor = false;
    [SerializeField] private bool EnableGameCrashInDevBuild = false;
    [SerializeField] private bool EnableGameCrashInBuild = true;
    [SerializeField] public CommandProperties[] Commands;

    [Serializable]
    public class CommandProperties
    {
        public ExtremeEngineConsoleCommand Command;
        public CommandTypes Permissions = CommandTypes.EnableInBuildAlways;

        public enum CommandTypes { OnlyEditor, EnableInDevolomentBuild, EnableInBuildWithCheatsOn, EnableInBuildAlways }
    }

    public CommandProperties.CommandTypes GetCommandPermission(ExtremeEngineConsoleCommand command)
    {
        for (int i = 0; i < Commands.Length; i++)
        {
            if (Commands[i].Command == command)
            {
                return Commands[i].Permissions;
            }
        }
        Debug.LogError("EXConsole properties cant find command to check permission!" + "Command:" + " " + command.ICommand);
        return CommandProperties.CommandTypes.EnableInBuildAlways;
    }

    public int GetMaximumSavedLogLines()
    {
        return MaximumSavedLogLines;
    }

    public bool CheckIfCanUseCommandPermission(CommandProperties.CommandTypes type, bool cheatsEnabled)
    {
        if ((type == CommandProperties.CommandTypes.OnlyEditor & Application.isEditor) || (type == CommandProperties.CommandTypes.EnableInDevolomentBuild & Debug.isDebugBuild) ||
            (type == CommandProperties.CommandTypes.EnableInBuildWithCheatsOn & cheatsEnabled) || (type == CommandProperties.CommandTypes.EnableInBuildAlways))
        {
            return true;
        }
        return false;
    }

    public ExtremeEngineDebugScreen Execute()
    {
        if (CheckIfCanExecute())
        {
            var debug = Instantiate(DebugScreen);
            debug.GetConsole().SetProperties(this);
            return debug;
        }
        return null;
    }

    private bool CheckIfCanExecute()
    {
        if ((ConsolePermissions == ConsolePermissionsTypes.OnlyEditor || ConsolePermissions == ConsolePermissionsTypes.EnableInDevolomentBuild || ConsolePermissions == ConsolePermissionsTypes.EnableInBuild) & Application.isEditor ||
            (ConsolePermissions == ConsolePermissionsTypes.EnableInDevolomentBuild || ConsolePermissions == ConsolePermissionsTypes.EnableInBuild) & !Application.isEditor & Debug.isDebugBuild ||
            ConsolePermissions == ConsolePermissionsTypes.EnableInBuild & !Application.isEditor & !Debug.isDebugBuild)
        {
            return true;
        }
        return false;
    }

    public bool CheckIfCanCrash()
    {
        if (Application.isEditor & EnableGameCrashInEditor || !Application.isEditor & Debug.isDebugBuild & EnableGameCrashInDevBuild || !Application.isEditor & !Debug.isDebugBuild & EnableGameCrashInBuild)
        {
            return true;
        }
        return false;
    }
}