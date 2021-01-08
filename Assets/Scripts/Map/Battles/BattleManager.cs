using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;
using Enemys;

public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject battlePanel = default;
    [SerializeField] GameObject commandPanel = default;
    [SerializeField] GameObject playerObj = default;

    PlayerCore player;
    EnemyCore enemy;

    public static BattleManager instance;
    private void Awake()
    {
        instance = this;
        battlePanel.SetActive(false);
        commandPanel.SetActive(false);
    }

    private void Start()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }
        player = playerObj.GetComponent<PlayerCore>();
    }

    public void SetupBattle(EnemyCore enemy)
    {
        // バトル画面を出す
        battlePanel.SetActive(true);
        commandPanel.SetActive(true);
        this.enemy = enemy;
        StartCoroutine(Battle());
    }

    IEnumerator Battle()
    {
        while (true)
        {
            yield return WaitPlayerCommand(); // ここでコマンドを受け取る？ Playerにコマンドをセットする
            // TODO:素早い順に行動
            int enemySp = enemy.Speed;
            int playerSp = player.Speed;
            if (playerSp > enemySp)
            {
                yield return player.Attack(enemy);
                if (enemy.IsDied())
                {
                    Debug.Log("Enemyの死亡");
                    break;
                }
                yield return enemy.Attack(player);
                if (player.IsDied())
                {
                    Debug.Log("Playerの死亡");
                    break;
                }
            }
            else
            {
                yield return enemy.Attack(player);
                if (player.IsDied())
                {
                    Debug.Log("Playerの死亡");
                    break;
                }
                yield return player.Attack(enemy);
                if (enemy.IsDied())
                {
                    Debug.Log("Enemyの死亡");
                    break;
                }
            }
            // HPが0になったらループを抜ける
        }
        EndBattle();
    }

    IEnumerator WaitPlayerCommand()
    {
        Debug.Log("コマンド入力待ち");
        commandPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    }




    void EndBattle()
    {
        battlePanel.SetActive(false);
        // playerを動けるようにする
        playerObj.GetComponent<PlayerMove>().canMove = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            EndBattle();
        }
    }

}
