using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    public class EnemyCore : MonoBehaviour, IDamageable
    {
        public Status status;

        public void Damage(int damage)
        {

        }

        public void Attack(IDamageable damageable)
        {
            damageable.Damage(status.at);
        }
    }

    public struct Status
    {
        public string name;
        int level;
        public int hp;
        int mp;
        int gold;
        int experiencePoint;

        public int at;
    }
}


