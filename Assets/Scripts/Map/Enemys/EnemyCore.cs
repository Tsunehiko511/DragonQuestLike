using System;
using System.Collections;
using UnityEngine;

namespace Enemys
{
    public enum MonsterType
    {
        Slime,
        SlimeBeth,
        Drakey,
        Ghost
    }

    [Serializable]
    public class EnemyCore : IDamageable, IAttackable, IDeadable
    {
        public MonsterType monsterType = default;
        public Sprite sprite = default;
        public Status status = new Status();

        public int HP
        {
            get => status.hp;
        }
        public int AT
        {
            get => status.at;
        }
        public int Speed
        {
            get => status.speed;
        }


        public void Damage(int damage)
        {

        }

        public IEnumerator Attack(IDamageable damageable)
        {
            damageable.Damage(status.at);
            return null;
        }

        public EnemyCore Clone()
        {
            return (EnemyCore)MemberwiseClone();
        }

        public bool IsDied()
        {
            return status.hp <= 0;
        }
    }

    [Serializable]
    public struct Status
    {
        public string name;
        public int level;
        public int hp;
        public int mp;
        public int gold;
        public int experiencePoint;
        public int at;
        public int speed;
    }
}


