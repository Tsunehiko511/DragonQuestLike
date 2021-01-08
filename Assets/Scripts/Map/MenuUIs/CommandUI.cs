using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUI : MonoBehaviour
{
    ButtonUI[] buttonUIs;
    void Start()
    {
        buttonUIs = GetComponentsInChildren<ButtonUI>();
    }
}
