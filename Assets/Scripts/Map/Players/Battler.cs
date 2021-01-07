using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battles
{
    public class BattlerBase : IDamageable, IAttackable
    {
        public struct Status
        {
            public string name;
            public int level;
            public int hp;
            public int at;
            public int speed;
        }

        public Status status;

        public void Damage(int damage)
        {
            throw new System.NotImplementedException();
        }

        public void Attack(IDamageable damageable)
        {
            damageable.Damage(status.at);
        }
    }

    public class Player : BattlerBase
    {
        
    }
}
