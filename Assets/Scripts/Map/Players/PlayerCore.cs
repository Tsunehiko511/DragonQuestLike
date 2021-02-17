using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// メッセージをどうやって出すのか？
// システムの方で処理を終わらせておいて、描画していく？

namespace Players
{
    public class PlayerCore : MonoBehaviour
    {
        
        [SerializeField] Battles.BattlerBase battler = default;
        IntReactiveProperty mp = new IntReactiveProperty(1);
        IntReactiveProperty gold = new IntReactiveProperty(1);
        IntReactiveProperty experiencePoint = new IntReactiveProperty(1);
        PLayerUI playerUI;
        public Battles.BattlerBase Battler
        {
            get => battler;
        }

        void Awake()
        {
            playerUI = GetComponent<PLayerUI>();
        }

        private void Start()
        {
            // UpdateUI();
        }

        public void UpdateUI()
        {
            playerUI.UpdateUI(battler);
        }

        public void UpdateUI(Character player)
        {
            playerUI.UpdateUI(player);
        }



        public void Attack()
        {
            battler.SetCommand(Battles.Commands.Attack);
        }
        public void MagicAction()
        {
            battler.SetCommand(Battles.Commands.Magic);
        }
        public void Escape()
        {
            battler.SetCommand(Battles.Commands.Escape);
        }
        public void UseTool()
        {
            battler.SetCommand(Battles.Commands.UseTool);
        }
    }
}

