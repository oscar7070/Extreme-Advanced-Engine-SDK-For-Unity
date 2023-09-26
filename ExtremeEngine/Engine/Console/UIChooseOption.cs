using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIChooseOption : MonoBehaviour
{

    private List<Option> Options = new();
    private struct Option
    {
        public string Text;
        public Button Button;

        public Option(string text, Button button)
        {
            Text = text;
            Button = button;
        }
    }

    [SerializeField] private ButtonUIProperties DefaultButton;
    [HideInInspector]public UnityEvent<int> OnChooseOption;
    private ScrollRect ScrollRect;

    private void Awake()
    {
        ScrollRect = GetComponent<ScrollRect>();
    }

    public void SetOptions(List<string> options)
    {
        ResetOptions();
        for (int i = 0; i < options.Count; i++)
        {
            var button = Instantiate(DefaultButton, ScrollRect.content);
            button.Text.text = options[i];
            var capturedButtonInt = i;
            button.Button.onClick.AddListener(delegate { OnClickCallBack(capturedButtonInt); }) ;
            Options.Add(new(options[i], button.Button));
        }
        Canvas.ForceUpdateCanvases();
        ScrollRect.verticalNormalizedPosition = 1;
        Canvas.ForceUpdateCanvases();
    }

    public string GetOption(int i)
    {
        return Options[i].Text;
    }

    public string[] GetAllOptions()
    {
        var toReturn = new string[Options.Count];
        for (int i = 0; i < toReturn.Length; i++)
        {
            toReturn[i] = Options[i].Text;
        }
        return toReturn;
    }

    private void OnClickCallBack(int option)
    {
        OnChooseOption.Invoke(option);
    }

    private void ResetOptions()
    {
        for (int i = 0; i < Options.Count; i++)
        {
            Destroy(Options[i].Button.gameObject);
        }
        Options.Clear();
    }
}
