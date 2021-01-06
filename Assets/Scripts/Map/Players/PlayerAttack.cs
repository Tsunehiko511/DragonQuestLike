using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    // 戦闘はBattlerで統一すると楽かも
    // Battlerはコマンドを受けて行動する
    // enemyであってもコマンドの割合を決めておけばいいかな？

    public class PlayerAttack : MonoBehaviour, IDamageable
    {

        PlayerCore playerCore;

        void Start()
        {
            playerCore = GetComponent<PlayerCore>();
        }

        public void Attack(IDamageable damageable)
        {
            damageable.Damage(playerCore.status.at);
        }

        public void Damage(int damage)
        {

        }
    }

}
