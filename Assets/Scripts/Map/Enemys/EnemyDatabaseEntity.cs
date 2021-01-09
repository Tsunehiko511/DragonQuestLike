using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    [CreateAssetMenu]
    public class EnemyDatabaseEntity : ScriptableObject
    {
        public List<EnemyCore> enemyList = new List<EnemyCore>();
    }
}
