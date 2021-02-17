using System;
using System.Collections;
using UnityEngine;
using Battles;
namespace Enemys
{
    public enum MonsterType
    {
        Slime,
        SlimeBeth,
        Drakey,
        Ghost,
        Wizard,
        Scorpion,
        ShimaGu,
    }

    [Serializable]
    public class EnemyCore
    {
        public MonsterType monsterType = default;
        public Sprite sprite = default;
        
        public BattlerBase battler;

        public EnemyCore(EnemyCore enemyCore)
        {
            this.battler = new BattlerBase(enemyCore.battler);
            this.monsterType = enemyCore.monsterType;
            this.sprite = enemyCore.sprite;
        }
    }
}


