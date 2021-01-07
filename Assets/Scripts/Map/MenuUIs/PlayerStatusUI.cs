using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Players;

namespace MenuUIs
{
    public class PlayerStatusUI : MonoBehaviour
    {
        [SerializeField] Text nameText = default;
        [SerializeField] Text levelText = default;
        [SerializeField] Text hpText = default;
        [SerializeField] Text mpText = default;
        [SerializeField] Text goldText = default;
        [SerializeField] Text exText = default;

        // ステータスの変化があった場合に更新する

        public void UpdateName(string value)
        {
            nameText.text = value;
        }
        public void UpdateLevel(string value)
        {
            levelText.text = value;
        }
        public void UpdateHP(string value)
        {
            hpText.text = value;
        }
        public void UpdateMP(string value)
        {
            mpText.text = value;
        }
        public void UpdateGold(string value)
        {
            goldText.text = value;
        }
        public void UpdateEx(string value)
        {
            exText.text = value;
        }

    }
}
