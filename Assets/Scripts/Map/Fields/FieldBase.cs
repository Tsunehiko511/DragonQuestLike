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

    public class FieldBase : MonoBehaviour
    {
        [SerializeField] Fieldtype fieldType = default;
        // モンスターを仕込む？
        [SerializeField] List<string> encountMonsterList = default;
        public List<string> EncountMonsterList
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

