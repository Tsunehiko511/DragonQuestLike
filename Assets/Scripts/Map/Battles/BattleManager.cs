using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Players;
using Enemys;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    // ---Event---
    public delegate IEnumerator EventCoroutin();
    public EventCoroutin OnScreenEffectEvent = default;
    public EventCoroutin OnPlayerTakeDamage = default;
    public EventCoroutin OnWaitMessage = default;

    // ---変数---
    [SerializeField] BattlePanel battlePanel = default;

    // EnemyCore enemy;
    [SerializeField] BattleCharacter enemyCharacter = default;

    PlayerCore playerCore;

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

    private void Awake()
    {
    }
    void Start()
    {
        // StartCoroutine(BattleCorutine());

        playerGroup = new Group();
        playerGroup.AddPlayer("しまづ", hp: 20, mp: 17, level: 1);

        battlePanel.gameObject.SetActive(false);
        SoundManager.instance.PlayBGM(SoundManager.BGM.Battle);
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
                    if (playerGroup.member.condition.IsAsleep())
                    {
                        command = playerGroup.member.commands[0];
                        commands.Add(command);
                        ChangePhase(BattlePhase.EnemyChoose);
                        break;
                    }
                    if (windowBattleCommand.IsClose)
                    {
                        windowBattleCommand.Open();
                        windowBattleCommand.ResetCommandIndex();
                    }
                    windowBattleCommand.SetInteractable(true);
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
                        ChangePhase(BattlePhase.EnemyChoose);
                    }
                    break;
                case BattlePhase.ChooseSubCommand:
                    windowBattleCommand.SetInteractable(true);
                    CommandMenu commandMenu = command as CommandMenu;
                    if (windowBattleMagicCommand.IsClose)
                    {
                        windowBattleMagicCommand.Open();
                        windowBattleCommand.ShowCursor();
                        windowBattleMagicCommand.Spawn(commandMenu.subs); // 場所が違う
                    }

                    yield return new WaitUntil(() => InputYES || InputNO);
                    if (InputNO)
                    {
                        SoundManager.instance.PlaySE(SoundManager.SE.Button);
                        windowBattleMagicCommand.Close();
                        ChangePhase(BattlePhase.ChooseCommand);
                        break;
                    }
                    // 何をやったのか?
                    Command cmd = commandMenu.subs[windowBattleMagicCommand.current];
                    if (cmd is CommandSpell)
                    {
                        CommandSpell commandSpell = commandMenu.subs[windowBattleMagicCommand.current] as CommandSpell;
                        // もしMPがなかったらアラートを出して、選び直し
                        if (commandSpell.CanExecute())
                        {
                            commands.Add(commandSpell);
                            windowBattleCommand.Close();
                            windowBattleMagicCommand.Close();
                            ChangePhase(BattlePhase.EnemyChoose);
                        }
                        else
                        {
                            windowBattleLog.AddText(commandSpell.resultMessage);
                            yield return WaitMessage();
                            ChangePhase(BattlePhase.ChooseSubCommand);
                        }
                    }
                    else if (cmd is CommandItem)
                    {
                        CommandItem commandItem = cmd as CommandItem;
                        if (commandItem.CanUseInBattle())
                        {
                            commands.Add(commandItem);
                            commandMenu.subs.Remove(commandItem);
                            windowBattleCommand.Close();
                            windowBattleMagicCommand.Close();
                            
                            ChangePhase(BattlePhase.EnemyChoose);
                        }
                        else
                        {
                            windowBattleLog.AddText(commandItem.resultMessage);
                            yield return WaitMessage();
                            ChangePhase(BattlePhase.ChooseSubCommand);
                        }
                    }


                    break;
                case BattlePhase.EnemyChoose:
                    // 敵の選択
                    command = enemyGroup.member.commands[0];
                    commands.Add(command);
                    ChangePhase(BattlePhase.Executing);
                    break;
                case BattlePhase.Executing:
                    yield return Execute();
                    break;
                case BattlePhase.Result:
                    yield return new WaitForSeconds(0.5f);
                    windowBattleLog.ShowVictoryText(enemyGroup.member);
                    (playerGroup.member as Player).AddGoldAndExp(enemyGroup.member.gold, enemyGroup.member.exp);
                    playerCore.UpdateUI(playerGroup.member);
                    yield return WaitMessage();
                    ChangePhase(BattlePhase.End);
                    break;
                case BattlePhase.End:
                    // TODOゲーム終了時にすることまとめ
                    yield return new WaitForSeconds(1f);
                    // battlePanel.gameObject.SetActive(false);
                    EndBattle();
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
    // 実行の前に目を異常状態の回復フェーズを作る？
    IEnumerator Execute()
    {
        commands.Sort((unitA, unitB) => unitB.user.agility - unitA.user.agility);

        // 適当な自分のターンを唱える？
        while (commands.Count > 0)
        {
            Command command = commands[0];
            commands.RemoveAt(0);

            if (command.user.condition.IsAsleep())
            {
                command.user.condition.Update();
                string effectMessage = command.user.condition.Check(command.user);
                windowBattleLog.AddText(effectMessage); // useMessageか？
            }
            else
            {
                command.Execute();
                windowBattleLog.AddText(command.useMessage); // useMessageか？
                yield return WaitMessage();

                if (command.shakeEffect && command.success)
                {
                    SoundManager.instance.PlaySE(SoundManager.SE.Attack);
                    if (command.target is Enemy)
                    {
                        enemyCharacter.TakeHit();
                    }
                    else
                    {
                        yield return OnPlayerTakeDamage();
                    }
                };
                if (command.blinkEffect && command.success) yield return OnScreenEffectEvent?.Invoke();
                
                windowBattleLog.AddText(command.resultMessage);
            }
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
            playerCore.UpdateUI(playerGroup.member);
        }
        if (playerGroup.member.condition.IsAsleep() == false)
        {
            windowBattleLog.AddText("コマンド？");
        }
        yield return WaitMessage();
        windowBattleCommand.ResetCommandIndex();
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

    [SerializeField] EncountEnemySO encountEnemySO = default;

    public void SetupBattle(MonsterType monsterType)
    {
        Debug.Log(monsterType);
        EnemyCore enemy = EnemyDatabaseEntity.Instance.Spawn(monsterType);

        // EnemyCore enemy = encountEnemySO.EncountEnemy;
        windowBattleCommand.Initialize();
        windowBattleMagicCommand.Initialize();
        enemyGroup = new Group();
        enemyCharacter.SetImage(enemy.sprite);

        // 出現してきた敵を生成
        enemyGroup.AddEnemy(enemy.battler.Name, hp: enemy.battler.HP, mp: enemy.battler.MP, level: 1, agility:enemy.battler.Speed, strength:enemy.battler.AT);
        playerGroup.member.ResetSetting();
        Debug.Log(enemyGroup.member);

        // Playerの持ち物と技を読み込む？その都度読み込む？
        playerGroup.member.AddCommand(new CommandAttack("こうげき", 5, VocabularyHelper.UseAttack, VocabularyHelper.SuccessPlayerAttack, VocabularyHelper.FailAttack, enemyGroup.member, shakeEffect: true));
        Command command = new CommandMenu("じゅもん");
        playerGroup.member.AddCommand(command);
        // 技の設定
        (command as CommandMenu).AddSub(new CommandSpell("ホイミ", baseDamage: -10, maxDamage: -17, mpCost: 4, VocabularyHelper.UseSpell, VocabularyHelper.Heal, VocabularyHelper.NotEnoughMP, target: playerGroup.member, blinkEffect: true));
        (command as CommandMenu).AddSub(new CommandSpell("ギラ", baseDamage: 5, maxDamage: 12, mpCost: 2, VocabularyHelper.UseSpell, VocabularyHelper.SuccessEnemyAttack, VocabularyHelper.FailAttack, target: enemyGroup.member, spellType: SpellType.Hurt, shakeEffect: true));
        (command as CommandMenu).AddSub(new CommandSpell("ラリホー", baseDamage: 0, maxDamage: 0, mpCost: 2, VocabularyHelper.UseSpell, VocabularyHelper.SuccessSleepSpell, VocabularyHelper.FailSleepSpell, target: enemyGroup.member, spellType: SpellType.Sleep, blinkEffect: true));
        (command as CommandMenu).AddSub(new CommandSpell("メラ", baseDamage: 50, maxDamage: 100, mpCost: 8, VocabularyHelper.UseSpell, VocabularyHelper.SuccessSleepSpell, VocabularyHelper.FailSleepSpell, target: enemyGroup.member, spellType: SpellType.Hurt, blinkEffect: true));
        // TODO:戦闘開始時には、ターゲットだけ指定するようにする
        playerGroup.member.AddCommand(new CommandEscape("にげる", VocabularyHelper.UseEscape, VocabularyHelper.SuccessEscape, VocabularyHelper.FailEscape, enemyGroup.member));
        // アイテムの実装
        command = new CommandMenu("どうぐ");
        playerGroup.member.AddCommand(command);
        (command as CommandMenu).AddSub(new CommandItem("たいまつ", "{0}は　{1}をつかった！", "HPを　{0}かいふくした！", "いまはつかえない", UsageType.FieldOnly));
        (command as CommandMenu).AddSub(new CommandItem("やくそう", "{0}は　{1}をつかった！", "HPを　{0}かいふくした！", "いまはつかえない", UsageType.Always));
        (command as CommandMenu).AddSub(new CommandItem("やくそう", "{0}は　{1}をつかった！", "HPを　{0}かいふくした！", "いまはつかえない", UsageType.Always));
        playerGroup.member.AddCommand(new CommandEscape("どうぐ", VocabularyHelper.UseItem, VocabularyHelper.SuccessUseItem, VocabularyHelper.FailUseItem, enemyGroup.member));




        enemyGroup.member.AddCommand(new CommandAttack("こうげき", 5, VocabularyHelper.UseAttack, VocabularyHelper.SuccessEnemyAttack, VocabularyHelper.FailAttack, playerGroup.member, shakeEffect: true));
        // enemyGroup.member.AddCommand(new CommandSpell("ラリホー", 0, 0, 2, "{0}は　ラリホーのじゅもんをとなえた！", "ネタ", "MPがたりない", target: playerGroup.member, spellType: SpellType.Sleep));


        playerCore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();
        playerCore.GetComponent<PlayerMove>().canMove = false;
        playerCore.UpdateUI(playerGroup.member);

        phase = BattlePhase.Initialize;
        StartCoroutine(BattleCorutine());
    }




    void EndBattle()
    {
        commands.Clear();
        battlePanel.gameObject.SetActive(false);

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
