using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalHeroStatusUI : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO = default;
    [SerializeField] Text hpText = default;
    [SerializeField] Text goldText = default;

    private void Start()
    {
        UpdateHP();
        UpdateGold();
    }

    public void UpdateHP()
    {
        hpText.text = string.Format("HP:{0}", playerStatusSO.HP);
    }

    public void UpdateGold()
    {
        Debug.Log("UpdateGold");
        goldText.text = string.Format("G:{0}", playerStatusSO.Gold);
    }

}
