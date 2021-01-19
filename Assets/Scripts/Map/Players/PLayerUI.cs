using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLayerUI : MonoBehaviour
{
    [SerializeField] Text levelText;
    [SerializeField] Text hpText;
    [SerializeField] Text mpText;
    [SerializeField] Text goldText;
    [SerializeField] Text exText;

    public void UpdateUI(Battles.BattlerBase player)
    {
        levelText.text = player.Level.ToString();
        hpText.text = player.HP.ToString();
        mpText.text = player.MP.ToString();
        goldText.text = player.Gold.ToString();
        exText.text = player.Ex.ToString();
    }
}
