using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Players
{
    public class EncountChecker : MonoBehaviour
    {
        // 移動しているとき、敵に遭遇したら一定確率で戦闘に移す
        // 城だったら確定で移動する TODO：現在も城なら？　城の１つ下から出現でOK

        int encount;
        const int ENCOUNT_TIME = 80;
        const int ENCOUNT_RATE = 30;
        PlayerMove playerMove;
        [System.Serializable]
        public class EncountEvent : UnityEvent<Enemys.MonsterType> { };
        [SerializeField] EncountEvent OnBattle = default;
        [SerializeField] EncountEnemySO encountEnemySO = default;

        public List<Enemys.MonsterType> EncountMonsterList { get; set; } = default;

        void Start()
        {
            playerMove = GetComponent<PlayerMove>();
        }

        void Update()
        {
            // 城だったら出会う
            if (playerMove.isMoving && EncountMonsterList !=null && EncountMonsterList.Count > 0)
            {
                CheckEncount();
            }
        }

        // 数回に1回エンカウントする
        void CheckEncount()
        {
            encount++;
            if (encount >= ENCOUNT_TIME)
            {
                Debug.Log(encount+"出会うかも");
                encount = 0;
                int rate = Random.Range(0, 100);
                if (rate < ENCOUNT_RATE)
                {
                    playerMove.canMove = false;
                    playerMove.isMoving = false;
                    int r = Random.Range(0, EncountMonsterList.Count);
                    Enemys.MonsterType monsterType = EncountMonsterList[r];
                    Debug.Log(monsterType + "に出会う");
                    // モンスター生成
                    OnBattle?.Invoke(monsterType);
                }
            }
        }
    }
}