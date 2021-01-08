using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    public class EnemyDatabase : MonoBehaviour
    {
        [SerializeField] EnemyDatabaseEntity database = default;

        public static EnemyDatabase instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public EnemyCore Spawn(MonsterType type)
        {
            EnemyCore enemy = database.enemyList.Find(x => x.monsterType == type);
            return enemy.Clone();
        }
    }
}
