using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] Text[] messageTexts = default;
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

    public void Init()
    {
        correntLine = 0;
        messages.Clear();
        messageList.Clear();
        foreach (Text text in messageTexts)
        {
            text.text = "";
        }
    }

    public void AddMessage(string message)
    {
        messages.Add(message);
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

    public IEnumerator BattleMessageAttack(string attacker, string defender, bool isPlayer)
    {
        if (isPlayer)
        {
            BattleMessagePlayerAttack(attacker, defender);
        }
        else
        {
            BattleMessageEnemyAttack(defender, attacker);
        }
        yield return ShowMessage();
    }

    void BattleMessagePlayerAttack(string playerName, string enemyName)
    {
        AddMessage(playerName + "の　こうげき！");
        AddMessage(enemyName + "に　3ポイントの");
        AddMessage("ダメージを　あたえた！");
    }

    void BattleMessageEnemyAttack(string playerName, string enemyName)
    {
        AddMessage("　"+enemyName + "の　こうげき！");
        AddMessage("　" + playerName + "は　2ポイントの");
        AddMessage("　" + "ダメージを　うけた！");
    }

    public IEnumerator BattleMessageEnemyDie(string enemyName, int point, int gold)
    {
        AddMessage(string.Format("{0}　をたおした！", enemyName));
        AddMessage(string.Format("けいけんち　{0}ポイントかくとく", point));
        AddMessage(string.Format("{0}ゴールドを　てにいれた！", gold));
        AddMessage("2ゴールドを　てにいれた！");
        yield return ShowMessage();
    }
}
