using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemys
{
    [CreateAssetMenu]
    public class EnemyDatabaseEntity : ScriptableObject
    {
        public List<EnemyCore> enemyList = new List<EnemyCore>();
        const string PATH = "Databases/EnemyDatabase";

        //MyScriptableObjectの実体
        private static EnemyDatabaseEntity instance;
        public static EnemyDatabaseEntity Instance
        {
            get
            {
                //初アクセス時にロードする
                if (instance == null)
                {
                    instance = Resources.Load<EnemyDatabaseEntity>(PATH);

                    //ロード出来なかった場合はエラーログを表示
                    if (instance == null)
                    {
                        Debug.LogError(PATH + " not found");
                    }
                }

                return instance;
            }
        }

        public EnemyCore Spawn(MonsterType type)
        {
            EnemyCore enemy = enemyList.Find(x => x.monsterType == type);
            return new EnemyCore(enemy);
        }

    }
}
