using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;
using Enemys;
using Battles;

public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject battlePanel = default;
    [SerializeField] GameObject commandPanel = default;
    [SerializeField] MessagePanel messagePanel = default;
    // EnemyCore enemy;

    BattlerBase player;
    BattlerBase enemy;
    // 敵を表示するもの:追加してやる？

    public static BattleManager instance;
    private void Awake()
    {
        instance = this;
        battlePanel.SetActive(false);
        commandPanel.SetActive(false);
    }

    private void Start()
    {
    }

    public void SetupBattle(EnemyCore enemy)
    {
        // バトル画面を出す
        battlePanel.SetActive(true);
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>().Battler;
        this.enemy = enemy.battler;
        messagePanel.Init();
        StartCoroutine(Battle());
    }

    IEnumerator Battle()
    {
        messagePanel.AddMessage(this.enemy.Name + "が　あらわれた！");
        // コマンド表示
        while (true)
        {
            messagePanel.AddMessage("コマンド？");
            messagePanel.AddMessage(MessagePanel.WAIT);
            yield return messagePanel.ShowMessage();
            yield return WaitPlayerCommand(); // ここでコマンドを受け取る？ Playerにコマンドをセットする

            BattlerBase first, second;

            if (player.Speed > enemy.Speed)
            {
                first = player;
                second = enemy;
            }
            else
            {
                first = enemy;
                second = player;
            }

            yield return first.SelectCommand(second);
            yield return messagePanel.BattleMessageAttack(first.Name, second.Name, first.IsPlayer);
            if (second.IsDied())
            {
                yield return new WaitForSeconds(0.2f);
                yield return messagePanel.BattleMessageEnemyDie(enemyName:enemy.Name, point:1, gold:2);
                yield return new WaitForSeconds(2f);
                break;
            }

            yield return new WaitForSeconds(0.5f);
            yield return second.SelectCommand(first);
            yield return messagePanel.BattleMessageAttack(second.Name, first.Name, second.IsPlayer);
            if (first.IsDied())
            {
                Debug.Log(first.Name + "の死亡");
                break;
            }
        }
        EndBattle();
    }


    IEnumerator WaitPlayerCommand()
    {
        commandPanel.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        commandPanel.SetActive(false);
    }




    public void EndBattle()
    {
        battlePanel.SetActive(false);
        // playerを動けるようにする
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            EndBattle();
        }
    }

}
