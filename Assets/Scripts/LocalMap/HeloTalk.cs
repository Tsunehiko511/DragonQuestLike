using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeloTalk : MonoBehaviour
{
    [SerializeField] LayerMask whatTalkLayer = default;

    HeroCore heroCore = default;

    private void Start()
    {
        heroCore = GetComponent<HeroCore>();
        heroCore.OnSpace += TalkTo;
    }
    // 人に話しかける:移動不可
    public void TalkTo(Vector3 position)
    {
        if (position == default)
        {
            return;
        }
        Collider2D hit = Physics2D.OverlapCircle(transform.position + position * 0.5f, 0.2f, whatTalkLayer);
        // Rayを飛ばして会話する
        if (hit != null)
        {
            heroCore.CanInput = false;
            hit.GetComponent<TownNPC>().Talked();
        }
    }
}
