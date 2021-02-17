using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;

namespace Maps
{
    enum Fieldtype
    {
        None,
        Grassland,
        Forest,
        Mountain,
        Cave,
        Sea,
    }

    public class EnemyEncountField : FieldBase
    {

        // 聞かれたから答えたのに どうやって作ってるんだろうね？
        // [SerializeField] Fieldtype fieldType = default;
        // モンスターを仕込む？ // モンスターの場合もあれば
        [SerializeField] List<Enemys.MonsterType> encountMonsterList = default;
        public List<Enemys.MonsterType> EncountMonsterList
        {
            get => encountMonsterList;
            set => encountMonsterList = value;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") == false)
            {
                return;
            }

            if (encountMonsterList.Count > 0)
            {
                // 敵がいるならテーブルを渡してやる
                EncountChecker player = collision.GetComponent<EncountChecker>();
                player.EncountMonsterList = encountMonsterList;
            }
        }

    }
}

