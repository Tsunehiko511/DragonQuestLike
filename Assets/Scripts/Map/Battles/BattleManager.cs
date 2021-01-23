using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;
using Enemys;
using Battles;

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
    }


    enum BattlePhase
    {
        Initialize,
        ChooseCommand,
        ChooseSubCommand,
        EnemyChoose,
        Executing,
        Result,
        End,
    }

    [SerializeField] BattlePhase phase;

    Group playerGroup;
    Group enemyGroup;
    [SerializeField] WindowBattleLog windowBattleLog = default;
    [SerializeField] WindowBattleCommand windowBattleCommand = default;
    [SerializeField] WindowBattleCommand windowBattleMagicCommand = default;

    void Start()
    {
        windowBattleCommand.Initialize();
        windowBattleMagicCommand.Initialize();
        playerGroup = new Group();
        enemyGroup = new Group();

        // 出現してきた敵を生成
        enemyGroup.AddEnemy("スライム", hp: 15, mp: 5, level: 1);
        playerGroup.AddCharacter("しまづ", hp: 20, mp: 7, level: 1);

        // Playerの持ち物と技を読み込む？その都度読み込む？
        playerGroup.member.AddCommand(new CommandAttack("こうげき", 5, "こうげき", "", "", enemyGroup.member));
        Command command = new CommandMenu("じゅもん", "", "", "失敗？？？");
        playerGroup.member.AddCommand(command);
        // 技の設定
        (command as CommandMenu).AddSub(new CommandSpell("ホイミー", baseDamage: -10, maxDamage: -17, mpCost: 4, "ホイミーを使った", "{0}は　{1}ポイントかいふくした", "MPがたりない", target: playerGroup.member));
        (command as CommandMenu).AddSub(new CommandSpell("ギラ", baseDamage: 5, maxDamage: 12, mpCost: 2, "ギラを使った", "ダメージ", "MPがたりない", target: enemyGroup.member, spellType: SpellType.Hurt));
        (command as CommandMenu).AddSub(new CommandSpell("ラリホー", 0, 0, 2, "ラリホー", "ネタ", "MPがたりない", target: enemyGroup.member, spellType: SpellType.Sleep));

        playerGroup.member.AddCommand(new CommandEscape("にげる", "にげようとした", "にげきった", "まわりこまれた！", enemyGroup.member));
        playerGroup.member.AddCommand(new CommandEscape("どうぐ", "にげようとした", "にげきった", "まわりこまれた！", enemyGroup.member));
        enemyGroup.member.AddCommand(new CommandAttack("こうげき", 5, "こうげき", "", "", playerGroup.member));


        playerCore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();
        playerCore.GetComponent<PlayerMove>().canMove = false;
        phase = BattlePhase.Initialize;
        StartCoroutine(BattleCorutine());
    }

    
    List<Command> commands = new List<Command>();

    bool InputYES
    {
        get => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X);
    }
    bool InputNO
    {
        get => Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Z);
    }

    IEnumerator BattleCorutine()
    {
        phase = BattlePhase.Initialize;
        Command command = null;

        while (true)
        {
            yield return null;
            switch (phase)
            {
                case BattlePhase.Initialize:
                    battlePanel.gameObject.SetActive(true);
                    windowBattleLog.Open();
                    windowBattleLog.ClearText();
                    windowBattleLog.AddText(VocabularyHelper.BattleStart(enemyGroup.member.name), breakline: false);
                    windowBattleLog.AddText("コマンド？");
                    yield return WaitMessage();
                    ChangePhase(BattlePhase.ChooseCommand);
                    break;
                case BattlePhase.ChooseCommand:
                    windowBattleCommand.SetInteractable(true);
                    windowBattleCommand.Open();
                    windowBattleCommand.ShowCursor();
                    yield return new WaitUntil(() => InputYES);
                    command = playerGroup.member.commands[windowBattleCommand.current];
                    if (command is CommandMenu)
                    {
                        windowBattleCommand.SetInteractable(false);
                        ChangePhase(BattlePhase.ChooseSubCommand);
                    }
                    else
                    {
                        commands.Add(command);
                        windowBattleCommand.Close();
                        ChangePhase(BattlePhase.Executing);
                    }
                    break;
                case BattlePhase.ChooseSubCommand:
                    windowBattleMagicCommand.Open();
                    CommandMenu commandMenu = command as CommandMenu;
                    windowBattleMagicCommand.Spawn(commandMenu.subs);

                    yield return new WaitUntil(() => InputYES || InputNO);
                    if (InputNO)
                    {
                        windowBattleMagicCommand.Close();
                        ChangePhase(BattlePhase.ChooseCommand);
                        break;
                    }
                    // 何をやったのか?
                    command = commandMenu.subs[windowBattleMagicCommand.current] as CommandSpell;
                    commands.Add(command);

                    windowBattleCommand.Close();
                    windowBattleMagicCommand.Close();
                    ChangePhase(BattlePhase.Executing);
                    break;
                case BattlePhase.Executing:
                    yield return Execute();
                    break;
                case BattlePhase.Result:
                    yield return new WaitForSeconds(0.5f);
                    windowBattleLog.ShowVictoryText(enemyGroup.member);
                    yield return WaitMessage();
                    ChangePhase(BattlePhase.End);
                    break;
                case BattlePhase.End:
                    // TODOゲーム終了時にすることまとめ
                    yield return new WaitForSeconds(1f);
                    battlePanel.gameObject.SetActive(false);
                    yield break;
            }
            yield return null;
        }
    }

    bool IsBattleOver()
    {
        // どちらか一方が死亡している
        return enemyGroup.Dead() || playerGroup.Dead();
    }
    IEnumerator Execute()
    {
        Debug.Log("コマンド？");
        while (commands.Count > 0)
        {
            Command command = commands[0];
            commands.RemoveAt(0);
            command.Execute();
            windowBattleLog.AddText(command.useMessage);
            yield return WaitMessage();
            windowBattleLog.AddText(command.resultMessage);
            yield return WaitMessage();
            if (IsBattleOver())
            {
                ChangePhase(BattlePhase.Result);
                yield break;
            }
            else if (command is CommandEscape)
            {
                if (command.success)
                {
                    windowBattleLog.AddText(command.resultMessage);
                    yield return WaitMessage();
                    ChangePhase(BattlePhase.End);
                    yield break;
                }
            }
            // PlayerのHPを反映
        }
        windowBattleLog.AddText("コマンド？");
        yield return WaitMessage();
        ChangePhase(BattlePhase.ChooseCommand);
    }

    IEnumerator WaitMessage()
    {
        while (windowBattleLog.IsIdle() == false)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    void ChangePhase(BattlePhase phase)
    {
        this.phase = phase;
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
        // 
    }




    void EndBattle()
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
