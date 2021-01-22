using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenuUI : MonoBehaviour
{
    [SerializeField] GameObject magicPanel = default;
    GameObject currentShowPanel;
    private void Awake()
    {
        magicPanel.SetActive(false);
    }

    public void OnSelectAttck()
    {
        // 攻撃コマンドを選択:
    }

    public void OnSelectMagic()
    {
        // じゅもん一覧を表示
        currentShowPanel = magicPanel;
        currentShowPanel.SetActive(true);
        // Playerの持っている魔法を表示
        // なければ
        // 次へボタンがある

        // 戦闘時の技
        // ホイミ　　　ギラ
        // ラリホー　　マホトーン
        // ベホイミ　　ベギマラ

        // 通常時の技
        // ホイミ　　　レミーラ
        // マホトーン　リレミント
        // ルーラ　　　トヘロス
        // ベホイミ　　---
        // 覚えた分だけ長くする？

    }

    public void OnSelectEscape()
    {
        // 逃げる:
    }

    public void OnSelectTool()
    {
        // どうぐ一覧を表示
    }

    public void OnClose()
    {
        currentShowPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnClose();
        }
    }

}
