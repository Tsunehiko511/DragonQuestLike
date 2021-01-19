using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;
using Enemys;
using Battles;
using System;

public class BattleManager : MonoBehaviour
{
    [SerializeField] BattlePanel battlePanel = default;
    [SerializeField] GameObject commandPanel = default;
    [SerializeField] MessagePanel messagePanel = default;
    // EnemyCore enemy;

    PlayerCore playerCore;
    BattlerBase player;
    BattlerBase enemy;
    // 敵を表示するもの:追加してやる？

    public static BattleManager instance;
    private void Awake()
    {
        instance = this;
        battlePanel.gameObject.SetActive(false);
        commandPanel.SetActive(false);
    }

    private void Start()
    {
    }

    public void SetupBattle(EnemyCore enemy)
    {
        // バトル画面を出す
        battlePanel.gameObject.SetActive(true);
        battlePanel.SetMoster(enemy.sprite);
        playerCore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();
        this.player = playerCore.Battler;
        this.enemy = enemy.battler;
        messagePanel.Init();
        StartCoroutine(Battle());
    }

    IEnumerator Battle()
    {
        messagePanel.AddMessage(this.enemy.Name + "が　あらわれた！");
        List<string> callBackMessages = new List<string>();
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

            bool isDead = false;
            // TODO:コマンド発動のメッセージを表示
            yield return BattlerAction(atttacker: first, reciever: second, r => isDead = r);
            if (isDead)
            {
                break;
            }

            yield return new WaitForSeconds(0.5f);
            yield return  BattlerAction(atttacker:second, reciever: first, r => isDead = r);
            if (isDead)
            {
                break;
            }

        }
        EndBattle();
    }

    IEnumerator BattlerAction(BattlerBase atttacker, BattlerBase reciever, Action<bool> isDied)
    {
        yield return atttacker.SelectCommand(reciever, messages => messagePanel.AddMessage(messages));
        yield return messagePanel.ShowMessage();
        playerCore.UpdateUI();
        bool result = reciever.IsDied();
        isDied(result);
        if (result)
        {
            yield return new WaitForSeconds(0.2f);
            yield return messagePanel.BattleMessageDie(reciever);
            if (atttacker.IsPlayer)
            {
                atttacker.Ex += reciever.Ex;
                atttacker.Gold += reciever.Gold;
            }
            playerCore.UpdateUI();
            yield return new WaitForSeconds(2f);

        }

    }

    IEnumerator WaitPlayerCommand()
    {
        commandPanel.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        commandPanel.SetActive(false);
    }




    public void EndBattle()
    {
        messagePanel.ResetTextPositions();
        battlePanel.gameObject.SetActive(false);
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
