using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Players
{
    public class EncountChecker : MonoBehaviour
    {
        // 移動しているとき、敵に遭遇したら一定確率で戦闘に移す

        int encount;
        const int ENCOUNT_TIME = 80;
        const int ENCOUNT_RATE = 20;
        PlayerMove playerMove;

        public List<string> EncountMonsterList { get; set; } = default;


        void Start()
        {
            playerMove = GetComponent<PlayerMove>();
        }

        void Update()
        {
            if (playerMove.isMoving && EncountMonsterList.Count > 0)
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
                encount = 0;
                int rate = Random.Range(0, 100);
                if (rate < ENCOUNT_RATE)
                {
                    playerMove.canMove = false;
                    playerMove.isMoving = false;
                    int r = Random.Range(0, EncountMonsterList.Count);
                    Debug.Log(EncountMonsterList[r] + "に出会う");
                    BattleManager.instance.SetupBattle(EncountMonsterList[r]);
                }
            }
        }
    }
}