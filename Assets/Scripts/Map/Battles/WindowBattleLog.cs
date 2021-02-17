using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowBattleLog : WindowBase
{
    [SerializeField] Text textField = default;
    string textToAdd;
    float textSpeed;
    float scrollSpeed;

    private const int TOTAL_LINES = 3;     // 行数
    private const int CHARS_PER_LINE = 22; // 文字数

    public override void Open()
    {
        base.Open();
        textSpeed = 0.05f;
        scrollSpeed = 0.1f;
    }

    public void ClearText()
    {
        textField.text = "";
        textToAdd = "";
    }

    public void AddText(string text, bool breakline = true)
    {
        bool invikeRunngin = textToAdd.Length > 0;
        string enter;
        if (breakline)
        {
            enter = "\n";
        }
        else
        {
            enter = "";
        }
        textToAdd += enter + text;
        // 動いていないなら
        if (!invikeRunngin)
        {
            SoundManager.instance.PlaySE(SoundManager.SE.Message);
            Invoke(nameof(AddToDisplay), textSpeed);
        }
    }

    public bool IsIdle()
    {

        return textToAdd == "";
    }

    void AddToDisplay()
    {

        // 1文字たす
        textField.text += textToAdd.Substring(0, 1);

        if (textToAdd.Length == 1)
        {
            textToAdd = ""; // 長さが1になったら、からにする？
        }
        else
        {
            bool full = Full();
            if (full) ScrollTextUp();
            textToAdd = textToAdd.Substring(1, textToAdd.Length - 1);
            if (full)
            {
                Invoke(nameof(AddToDisplay), textSpeed + scrollSpeed);
            }
            else
            {
                Invoke(nameof(AddToDisplay), textSpeed);
            }
        }
    }

    

    // 行数を数えて、TOTAL_LINESを超えていればtrue
    bool Full()
    {
        int lines = 0;
        // 1文字ずつ調べる
        for (int i = 0, charaCount = 0; i < textField.text.Length; i++)
        {

            if (textField.text.ToCharArray()[i] == '\n')
            {
                // 改行があれば数える
                lines++;
                charaCount = 0;
            }
            else
            {
                charaCount++;
                if (charaCount >= CHARS_PER_LINE)
                {
                    // 1行分文字が埋まっていても行数として数える
                    lines++;
                    charaCount = 0;
                }
            }
        }
        return lines >= TOTAL_LINES;
    }

    //TODO:4行になってしまうことがある
    void ScrollTextUp()
    {
        // 1行目を削除する => textField.text.Substring(最初の行の文字数, 全ての文字数 - 最初の行の文字数);

        // 最初の行の文字数
        // 改行してるところまでか、１行分
        int charCountOfFirstLine = textField.text.IndexOf("\n") + 1;
        if (charCountOfFirstLine > CHARS_PER_LINE) 
        {
            // 次の行にいってたら、1行分+1
            charCountOfFirstLine = CHARS_PER_LINE + 1;
        }
        string subString = textField.text.Substring(charCountOfFirstLine, textField.text.Length - charCountOfFirstLine);
        textField.text = subString;
    }

    public void ShowVictoryText(Character enemy)
    {
        AddText(VocabularyHelper.Victory(enemy.name));
        AddText(VocabularyHelper.Exp(enemy.exp));
        AddText(VocabularyHelper.Gold(enemy.gold));
    }
}


public class WindowBase : MonoBehaviour
{
    public GameObject windowObj;

    public virtual void Initialize()
    {
        // windowObj.SetActive(false);
    }

    public virtual void Open()
    {
        windowObj.SetActive(true);
    }
    public virtual void Close()
    {
        windowObj.SetActive(false);
    }

    public bool IsClose
    {
        get => !windowObj.activeSelf;
    }

}