using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    [TextArea]
    public string[] sentences; // 文章を格納する
    [SerializeField] Text messageText = default;   // uiTextへの参照
    // [SerializeField] TextAsset messageText = default;   // uiTextへの参照

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharDisplay = 0.05f;   // 1文字の表示にかける時間

    private int currentSentenceNum = 0; //現在表示している文章番号
    private string currentSentence = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeBeganDisplay = 1;         // 文字列の表示を開始した時間
    private int lastUpdateCharCount = -1;       // 表示中の文字数

    bool OnNext = false;

    string message = string.Empty;

    void Start()
    {
        SetNextSentence();
    }


    public void Update()
    {
        OnNext = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // message = messageText.text;
            OnNext = true;

        }
        // 文章の表示完了 / 未完了
        if (IsDisplayComplete())
        {
            //最後の文章ではない & ボタンが押された
            if (currentSentenceNum < sentences.Length && OnNext)
            {
                SetNextSentence();
            }
            else if (currentSentenceNum >= sentences.Length)
            {
                currentSentenceNum = 0;
                if (CountOf(messageText.text, "\n") == 3)
                {
                    RemoveTextOneLine();
                }
            }
        }
        else
        {
            if (OnNext)
            {
                timeUntilDisplay = 0;
            }
        }

        //表示される文字数を計算
        int displayCharCount = (int)(Mathf.Clamp01((Time.time - timeBeganDisplay) / timeUntilDisplay) * currentSentence.Length);
        //表示される文字数が表示している文字数と違う
        if (displayCharCount != lastUpdateCharCount)
        {
            messageText.text = currentSentence.Substring(0, displayCharCount);
            //表示している文字数の更新
            lastUpdateCharCount = displayCharCount;
        }
    }

    void RemoveTextOneLine()
    {
        string tmpText = messageText.text;
        int p = tmpText.IndexOf("\n");
        if (p < 0) p = tmpText.Length;
        string outStr = tmpText.Substring(p, tmpText.Length - p);
        messageText.text = outStr;
    }

    public int CountOf(string target, params string[] strArray)
    {
        int count = 0;

        foreach (string str in strArray)
        {
            int index = target.IndexOf(str, 0);
            while (index != -1)
            {
                count++;
                index = target.IndexOf(str, index + str.Length);
            }
        }

        return count;
    }

    // 次の文章をセットする
    void SetNextSentence()
    {
        currentSentence = sentences[currentSentenceNum];
        timeUntilDisplay = currentSentence.Length * intervalForCharDisplay;
        timeBeganDisplay = Time.time;
        currentSentenceNum++;
        lastUpdateCharCount = 0;
    }

    bool IsDisplayComplete()
    {
        return Time.time > timeBeganDisplay + timeUntilDisplay; //※2
    }
}