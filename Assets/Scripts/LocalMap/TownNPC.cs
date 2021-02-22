using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TownNPC : EventMove
{
    [SerializeField] TalkTableSO talkTableSO = default;
    [System.Serializable]public class TalkEvent : UnityEvent<string> { };
    [SerializeField] TalkEvent OnTalk = default;
    [SerializeField] UnityEvent NPCEvent = default;

    // 回復させる:Playerの関数を実行する どうやって？


    // fungusみたいなシステムをこいつ自身に組む

    // 話しかけられる
    public void Talked()
    {
        OnTalk?.Invoke(talkTableSO.MessageText);
        HealHero();
    }

    // Yesを選択した場合に実行した
    public void HealHero()
    {
        NPCEvent?.Invoke();
    }

}
