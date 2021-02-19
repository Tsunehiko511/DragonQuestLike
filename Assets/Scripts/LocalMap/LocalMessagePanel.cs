using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocalMessagePanel : MonoBehaviour
{
    [SerializeField] GameObject panel = default;
    [SerializeField] Text messageText = default;
    [SerializeField] UnityEvent OnCompleted = default;
    

    bool isTalking;


    public void ShowMessage(string message)
    {
        if (isTalking)
        {
            return;
        }
        StartCoroutine(ShowText(message));
    }

    IEnumerator ShowText(string message)
    {
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
        panel.SetActive(false);
        OnCompleted?.Invoke();
    }
}
