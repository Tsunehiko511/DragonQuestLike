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

        public IEnumerator Attack(IDamageable damageable)
        {
            damageable.Damage(status.at);
            return null;
        }
    }

    public class Player : BattlerBase
    {
        // ステータスを表示するものが必要
    }
}
