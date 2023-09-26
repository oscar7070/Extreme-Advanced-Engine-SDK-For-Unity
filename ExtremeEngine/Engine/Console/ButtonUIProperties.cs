using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUIProperties : MonoBehaviour
{

    [HideInInspector]public Button Button;
    public TextMeshProUGUI Text;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }
}
