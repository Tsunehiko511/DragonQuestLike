using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;
using Enemys;

public class BattleManager : MonoBehaviour
{
    [SerializeField] GameObject battlePanel = default;
    [SerializeField] GameObject playerObj = default;

    PlayerAttack player;
    EnemyCore enemy;

    public static BattleManager instance;
    private void Awake()
    {
        instance = this;
        battlePanel.SetActive(false);
    }

    private void Start()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }
        player = playerObj.GetComponent<PlayerAttack>();
    }

    public void SetupBattle(string enemyName)
    {
        // バトル画面を出す
        battlePanel.SetActive(true);
        enemy.status.name = enemyName;
        StartCoroutine(Battle());
    }

    IEnumerator Battle()
    {
        while (true)
        {
            yield return WaitPlayerCommand(); // ここでコマンドを受け取る？ Playerにコマンドをセットする

            // TODO:素早い順に行動
            int enemySp = 10;
            int playerSp = 20;
            if (playerSp > enemySp)
            {
                player.Attack(enemy);
                enemy.Attack(player);
            }
            else
            {
                enemy.Attack(player);
                player.Attack(enemy);
            }
            // HPが0になったらループを抜ける
        }
        EndBattle();
    }

    IEnumerator WaitPlayerCommand()
    {
        yield return new WaitForSeconds(2f);
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
