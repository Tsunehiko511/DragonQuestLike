using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TownNPC : MonoBehaviour
{
    [SerializeField] TalkTableSO talkTableSO = default;
    [System.Serializable]public class TalkEvent : UnityEvent<string> { };
    [SerializeField] TalkEvent OnTalk = default;


    // 話しかけられる
    public void Talked()
    {
        OnTalk?.Invoke(talkTableSO.MessageText);
    }
}
