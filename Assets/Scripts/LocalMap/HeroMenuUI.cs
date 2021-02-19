using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMenuUI : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO = default;

    public void ShowStatusAndCommand()
    {
        ShowStatus();
        ShowCommand();
    }

    // ステータスを見せる
    void ShowStatus()
    {
        playerStatusSO.DebugLog();
    }

    // コマンドを見せる
    void ShowCommand()
    {
        // ステータス
        Debug.Log("コマンド表示");
    }
}
