﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class TalkTableSO : ScriptableObject
{
    [TextArea][SerializeField] string messageText = default;
    [SerializeField] UnityEvent unityEvent = default;
    public string MessageText
    {
        get => messageText;
    }

    // 選択肢が出るパターンもあるが、それはメッセージの方にコマンドを入れておけばOKかと
    // ここで全部のテキストを入れてしまうのがいいかな
    // マスターデータとして別のもので管理するものあり.エディタ拡張で後から差し替えればOKかと
}