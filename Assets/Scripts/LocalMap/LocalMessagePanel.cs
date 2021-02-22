using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MoonSharp.Interpreter;

// ランダムエンカウント
// もし、モンスターがいる場所を歩いていると、遭遇する
[MoonSharpUserData]
public class LocalMessagePanel : LuaInterpreterHandlerBase
{
    [SerializeField] GameObject panel = default;
    [SerializeField] Text messageText = default;
    [SerializeField] UnityEvent OnCompleted = default;
    

    bool isTalking;


    public void ShowMessage(string message, bool close = false)
    {
        if (isTalking)
        {
            return;
        }
        StartCoroutine(ShowText(message, close));
    }

    public void Close()
    {
        flag = false;
        panel.SetActive(false);
        flag = true;
    }

    IEnumerator ShowText(string message, bool close = false)
    {
        flag = false;
        isTalking = true;
        messageText.text = "";
        panel.SetActive(true);
        foreach (char word in message)
        {
            yield return new WaitForSeconds(0.05f);
            messageText.text += word;
        }
        isTalking = false;
        yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.Space));
        panel.SetActive(close);
        OnCompleted?.Invoke();
        flag = true;
    }
}
