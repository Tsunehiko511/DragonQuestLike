﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] Text[] messageTexts = default;
    Vector2[] defaultTextPosition = new Vector2[3];
    public const string WAIT = "WAIT";
    public List<string> messages = new List<string>()
    {
        "スライムベスが あらわれた！",
        "コマンド？",
        WAIT,
        "44444444",
        "55555555",
        "66666666",
        WAIT,
    };

    List<string> messageList = new List<string>();

    int correntLine = 0;

    private void Awake()
    {
        for (int i=0; i<defaultTextPosition.Length; i++)
        {
            defaultTextPosition[i] = messageTexts[i].transform.position;
        }
    }

    public void Init()
    {
        correntLine = 0;
        messages.Clear();
        messageList.Clear();
        foreach (Text text in messageTexts)
        {
            text.text = "";
        }
        for (int i = 0; i < defaultTextPosition.Length; i++)
        {
            defaultTextPosition[i] = messageTexts[i].transform.position;
        }
    }




    public void AddMessage(string message)
    {
        messages.Add(message);
    }
    public void AddMessage(List<string> messages)
    {
        Debug.Log(messages);
        Debug.Log(messages.Count);
        foreach (string message in messages)
        {
            this.messages.Add(message);
        }
    }

    public IEnumerator ShowMessage()
    {
        yield return ChangeMessage();
    }

    IEnumerator ChangeMessage()
    {
        while (IsLoop())
        {
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
        }
    }

    bool IsLoop()
    {
        if (correntLine >= messages.Count)
        {
            return false;
        }

        if (messages[correntLine] == WAIT)
        {
            correntLine++;
            return false;
        }
        return true;
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
        foreach (Text text in messageTexts)
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
        for (int i = 0; i < length; i++)
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


    public IEnumerator BattleMessageDie(Battles.BattlerBase battler)
    {
        if (battler.IsPlayer)
        {
            AddMessage(string.Format("{0}　はたおれてしまった！", battler.Name));
        }
        else
        {
            // Playerにも反映する必要がある
            AddMessage(string.Format("{0}　をたおした！", battler.Name));
            AddMessage(string.Format("けいけんち　{0}ポイントかくとく", battler.Ex));
            AddMessage(string.Format("{0}ゴールドを　てにいれた！", battler.Gold));
        }
        yield return ShowMessage();
    }


    public IEnumerator BattleMessageEnemyDie(string enemyName, int point, int gold)
    {
        AddMessage(string.Format("{0}　をたおした！", enemyName));
        AddMessage(string.Format("けいけんち　{0}ポイントかくとく", point));
        AddMessage(string.Format("{0}ゴールドを　てにいれた！", gold));
        yield return ShowMessage();
    }

    public void ResetTextPositions()
    {
        for (int i = 0; i < defaultTextPosition.Length; i++)
        {
            messageTexts[i].transform.position = defaultTextPosition[i];
        }

    }
}
