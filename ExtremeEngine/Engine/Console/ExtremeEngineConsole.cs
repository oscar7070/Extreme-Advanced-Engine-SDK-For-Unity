using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class ExtremeEngineConsole : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI WindowTitleText;
    [SerializeField] private TextMeshProUGUI ViewerText;
    [SerializeField] private ScrollRect ViewerScrollRect;
    [SerializeField] private TMP_InputField Input;
    [SerializeField] private Button SubmitInputButton;
    //[SerializeField] private Texture2D CustomCursor;
    [SerializeField] private Texture2D CursorOnCrash;
    [SerializeField] private Vector2 CursorHotSpot = Vector2.zero;
    [SerializeField] private UIChooseOption Helper;
    private ExtremeEngineConsoleProperties Properties;
    private bool CheatsEnabled = false;
    [SerializeField] private InputAction ToggleConsoleVisibilityAction;

    public void SetCheatsEnabled(bool enabled)
    {
        CheatsEnabled = enabled;
    }

    public bool GetCheatsEnabled()
    {
        return CheatsEnabled;
    }

    public void SetProperties(ExtremeEngineConsoleProperties properties)
    {
        Properties = properties;
    }

    public ExtremeEngineConsoleProperties GetProperties()
    {
        return Properties;
    }

    public void Start()
    {
        WindowTitleText.text = ExtremeEngineData.EngineNameShort + " Console";
        ViewerText.text = null;
        Input.text = null;
        Input.onSubmit.AddListener(OnSubmitInput);
        Input.onValueChanged.AddListener(OnInputChanged);
        Input.Select();
        Helper.OnChooseOption.AddListener(OnHelperChoose);
        ToggleConsoleVisibilityAction.Enable();
        ToggleConsoleVisibilityAction.performed += ToggleConsoleVisibility;

        Debug.Log(ExtremeEngineData.EngineName + " console started to work");
        HideConsole();
        Debug.Log(ExtremeEngineData.EngineName + " console Loaded! with " + Properties.Commands.Length + " Commands.");
    }

    public void SubmitInput()
    {
        OnSubmitInput(Input.text);
    }

    #region RemoveCallBack
    private void OnDestroy()
    {
        ExtremeEngineConsoleRunner.ConsoleTextReloaded -= OnReloadView;
    }
    #endregion

    public void HideConsole()
    {
        ExtremeEngineConsoleRunner.ConsoleTextReloaded -= OnReloadView;
        OnReloadView("");
        if (ExtremeEngineConsoleRunner.GetIfCrashed())
        {
            AutoApplicatonQuit.Execute();
        }
        gameObject.SetActive(false);
    }

    public void ShowConsole(bool isCrash = false)
    {
        OnReloadView(ExtremeEngineConsoleRunner.GetConsoleText());
        ExtremeEngineConsoleRunner.ConsoleTextReloaded += OnReloadView;
        gameObject.SetActive(true);
        if (isCrash)
        {
            Input.interactable = false;
            SubmitInputButton.interactable = false;
            GetComponent<RectTransform>().offsetMin = new(0, 0);
            SetCrashCursor();
        }
        SelectInputEnd();
    }

    public void SetCrashCursor()
    {
        Cursor.SetCursor(CursorOnCrash, CursorHotSpot, CursorMode.Auto);
    }

    private void ToggleConsoleVisibility(CallbackContext ctx)
    {
        if (gameObject.activeInHierarchy)
        {
            HideConsole();
        }
        else
        {
            ShowConsole();
        }
    }

    private void OnReloadView(string text)
    {
        ViewerText.text = text;
    }

    private void OnHelperChoose(int i)
    {
        Input.text = Helper.GetOption(i);
        SelectInputEnd();
    }

    private void SelectInputEnd()
    {
        Input.Select();
        Input.onFocusSelectAll = false;
        Input.MoveTextEnd(false);
    }

    private void OnInputChanged(string input)
    {
        var allCommands = Properties.Commands;
        string firstWord = GetFirstWord(input);
        List<string> options = new();
        if (!string.IsNullOrWhiteSpace(firstWord))
        {
            var charArray = firstWord.ToLower().ToCharArray();
            for (int i = 0; i < allCommands.Length; i++)
            {
                var commandChar = allCommands[i].Command.ICommand.ToLower().ToCharArray();
                bool found = true;
                if (charArray.Length <= commandChar.Length)
                {
                    for (int c = 0; c < charArray.Length; c++)
                    {
                        if (charArray[c] != commandChar[c])
                        {
                            found = false;
                            break;
                        }
                    }
                }
                else
                {
                    found = false;
                }
                if (found)
                {
                    options.Add(allCommands[i].Command.ICommand);
                }
            }
        }
        Helper.SetOptions(options);
    }

    private void OnSubmitInput(string input)
    {
        var allCommands = Properties.Commands;
        string firstWord = GetFirstWord(input);
        string[] commandProperties = SplitAndDeleteFirstWord(input);
        bool commandFound = false;
        for (int i = 0; i < allCommands.Length; i++)
        {
            if (firstWord == allCommands[i].Command.ICommand)
            {
                commandFound = true;
                //Check command permission
                ExtremeEngineConsoleProperties.CommandProperties.CommandTypes permission = Properties.GetCommandPermission(allCommands[i].Command);
                if (Properties.CheckIfCanUseCommandPermission(permission, CheatsEnabled))
                {
                    //Execute & check for params
                    bool end = allCommands[i].Command.OnCommandExecute(commandProperties);
                    /*if (end)
                    {
                        string FullInput = firstWord + " ";
                        for (int c = 0; c < commandProperties.Length; c++)
                        {
                            FullInput += commandProperties[c];
                        }
                        Debug.Log(FullInput);
                    }*/
                    if (!end)
                    {
                        Debug.Log("Command " + "<<noparse>" + input + "</noparse>>" + " exist but your params not valid.");
                    }
                    else
                    {
                        Input.text = null;
                    }
                }
                else
                {
                    Debug.Log("Command: <" + allCommands[i].Command.ICommand + ">" + " need permission: <" + permission.ToString() + ">");
                }
                break;
            }
        }
        if (!commandFound)
        {
            if (input != "")
            {
                Debug.Log("Command " + "<<noparse>" + input + "</noparse>>" + " not exist.");
            }
            else
            {
                Debug.Log("Your input is empty please write in the input field!");
            }
        }
        Canvas.ForceUpdateCanvases();
        ViewerScrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

    private string GetFirstWord(string str)
    {
        return str.Split(" ")[0];
    }

    private static string[] SplitAndDeleteFirstWord(string words)
    {
        if (words.Length > 0 & ContainsMoreThenOneWord(words))
        {
            int i = words.IndexOf(" ") + 1;
            string str = words.Substring(i);
            return str.Split(" ", StringSplitOptions.RemoveEmptyEntries); ;
        }
        return new string[0];
    }

    private static bool ContainsMoreThenOneWord(string str)
    {
        return str.Split(" ".ToCharArray()).Length > 1;
    }
}