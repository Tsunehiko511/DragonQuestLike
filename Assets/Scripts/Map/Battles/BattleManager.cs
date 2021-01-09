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
        StartCoroutine(Battle());
    }

    IEnumerator Battle()
    {
        while (true)
        {
            yield return WaitPlayerCommand(); // ここでコマンドを受け取る？ Playerにコマンドをセットする

            BattlerBase first, second;
            Debug.Log(player);
            Debug.Log(enemy);
            Debug.Log(enemy.Speed);
            Debug.Log(player.Speed);

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
            if (second.IsDied())
            {
                Debug.Log(second.Name+"の死亡");
                break;
            }

            yield return second.SelectCommand(first);
            if (first.IsDied())
            {
                Debug.Log(first.Name + "の死亡");
                break;
            }
            yield return new WaitForSeconds(2f);
            // HPが0になったらループを抜ける
        }
        EndBattle();
    }

    /*
    IEnumerator AttackAction(BattlerBase attacker, BatteryStatus defender)
    {
        attacker.Attack(defender);
        yield return new WaitForEndOfFrame(1f);

        if (defender.IsDied())
        {
            isDead = true;
            Debug.Log(second.Name + "の死亡");
        }
    }
    */


    IEnumerator WaitPlayerCommand()
    {
        Debug.Log("コマンド入力待ち");
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
