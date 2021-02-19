using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMessagePanel : MonoBehaviour
{
    // public const 

    public void ShowMessage(string message)
    {
        foreach (char word in message)
        {
            if (word == MessageParam.selectMark)
            {
                Debug.Log("選択肢を出す");
            }
        }
        // テーブルがあってそれを流す？
        Debug.Log(message);
    }
}
