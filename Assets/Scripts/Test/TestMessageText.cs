using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestMessageText : MonoBehaviour
{
    [SerializeField] Text[] messageTexts = default;
    string[] messages =
    {
        "11111111",
        "22222222",
        "33333333",
        "44444444",
        "55555555",
    };

    List<string> messageList = new List<string>();

    int correntLine = 0;
    bool isShowing;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isShowing == false)
        {
            StartCoroutine(ChangeMessage());
        }
    }

    IEnumerator ChangeMessage()
    {
        isShowing = true;
        // もし3文字以上なら
        if (messageList.Count == 3)
        {
            messageList.RemoveAt(0);
            RemoveFirstLine();
            MoveLines();
        }
        messageList.Add(messages[correntLine]);

        yield return new WaitForSeconds(0.15f);
        yield return AddLine(messages[correntLine]);
        correntLine++;
        correntLine %= messages.Length;
        isShowing = false;
    }


    IEnumerator ShowCharacterFor(Text showText, string message)
    {
        int count = 0;
        while (count < message.Length)
        {
            showText.text += message[count]; ;
            yield return new WaitForSeconds(0.04f);
            count++;
        }
        showText.text += "\n";
    }

    void RemoveFirstLine()
    {
        Text highestText = messageTexts[0];
        foreach(Text text in messageTexts)
        {
            if (highestText.transform.position.y < text.transform.position.y)
            {
                highestText = text;
            }
        }
        highestText.text = "";
    }

    IEnumerator AddLine(string message)
    {
        Text lowestText = messageTexts[0];
        foreach (Text text in messageTexts)
        {
            if (text.text == "")
            {
                lowestText = text;
                break;
            }
            if (lowestText.transform.position.y > text.transform.position.y)
            {
                lowestText = text;
            }
        }
        yield return ShowCharacterFor(lowestText, message);
    }
    // 上に移動させる: Text自体をかえる？
    void MoveLines()
    {
        int length = messageTexts.Length;
        float[] positions = new float[length];
        for (int i=0; i< length; i++)
        {
            positions[i] = messageTexts[i].transform.position.y;
        }

        for (int i = 0; i < length; i++)
        {
            int moveIndex = i - 1;
            if (moveIndex < 0)
            {
                moveIndex = length - 1;
            }
            messageTexts[i].transform.DOMoveY(positions[moveIndex], 0.1f).SetEase(Ease.Linear);
        }
    }
}
