using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroCore : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO;
    [SerializeField] UnityEvent OnTalk = new UnityEvent();
    [SerializeField] UnityEvent OnUseGold = new UnityEvent();
    [SerializeField] UnityEvent OnGetGold = new UnityEvent();
    [SerializeField] UnityEvent OnHeal = new UnityEvent();

    // 街ですること

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TalkTo();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UseGold(100);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetGold(50);
        }

    }


    // 人に話しかける:移動不可
    public void TalkTo()
    {
        GameObject.Find("NPC").GetComponent<TownNPC>().Talked();
        OnTalk?.Invoke();
    }

    // ものを買う:Gold減少
    public void UseGold(int amount)
    {
        playerStatusSO.Gold -= amount;
        OnUseGold?.Invoke();
    }
    // ものを売る:Gold増加
    public void GetGold(int amount)
    {
        playerStatusSO.Gold += amount;
        OnGetGold?.Invoke();
    }

    // 回復:HP上昇
    public void Heal(int amount)
    {
        playerStatusSO.HP += amount;
        OnHeal?.Invoke();
    }
}
