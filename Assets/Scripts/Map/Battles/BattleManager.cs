using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;
using Enemys;
using Battles;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    [SerializeField] BattlePanel battlePanel = default;
    [SerializeField] GameObject commandPanel = default;
    [SerializeField] MessagePanel messagePanel = default;
    // EnemyCore enemy;
    [SerializeField] BattleCharacter enemyCharacter = default;

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
        SoundManager.instance.PlayBGM(SoundManager.BGM.Battle);
        // Camera.main.
        windowBattleCommand.Initialize();
        windowBattleMagicCommand.Initialize();
        playerGroup = new Group();
        enemyGroup = new Group();

        // 出現してきた敵を生成
        enemyGroup.AddEnemy("スライム", hp: 15, mp: 5, level: 1);
        playerGroup.AddCharacter("しまづ", hp: 20, mp: 7, level: 1);

        // Playerの持ち物と技を読み込む？その都度読み込む？
        playerGroup.member.AddCommand(new CommandAttack("こうげき", 5, VocabularyHelper.UseAttack, VocabularyHelper.SuccessPlayerAttack, VocabularyHelper.FailAttack, enemyGroup.member, shakeEffect: true));
        Command command = new CommandMenu("じゅもん");
        playerGroup.member.AddCommand(command);
        // 技の設定
        (command as CommandMenu).AddSub(new CommandSpell("ホイミ", baseDamage: -10, maxDamage: -17, mpCost: 4, VocabularyHelper.UseSpell, VocabularyHelper.Heal, VocabularyHelper.NotEnoughMP, target: playerGroup.member, blinkEffect:true));
        (command as CommandMenu).AddSub(new CommandSpell("ギラ", baseDamage: 5, maxDamage: 12, mpCost: 2, VocabularyHelper.UseSpell, VocabularyHelper.SuccessEnemyAttack, VocabularyHelper.FailAttack, target: enemyGroup.member, spellType: SpellType.Hurt, shakeEffect:true));
        (command as CommandMenu).AddSub(new CommandSpell("ラリホー", baseDamage: 0, maxDamage: 0, mpCost: 2, VocabularyHelper.UseSpell, VocabularyHelper.SuccessSleepSpell, VocabularyHelper.FailSleepSpell, target: enemyGroup.member, spellType: SpellType.Sleep, blinkEffect:true));

        playerGroup.member.AddCommand(new CommandEscape("にげる", VocabularyHelper.UseEscape, VocabularyHelper.SuccessEscape, VocabularyHelper.FailSleepSpell, enemyGroup.member));
        // アイテムの実装
        command = new CommandMenu("どうぐ");
        playerGroup.member.AddCommand(command);
        (command as CommandMenu).AddSub(new CommandItem("たいまつ", "{0}は　{1}をつかった！", "HPを　{0}かいふくした！", "いまはつかえない", UsageType.FieldOnly));
        (command as CommandMenu).AddSub(new CommandItem("やくそう", "{0}は　{1}をつかった！", "HPを　{0}かいふくした！", "いまはつかえない", UsageType.Always));
        (command as CommandMenu).AddSub(new CommandItem("やくそう", "{0}は　{1}をつかった！", "HPを　{0}かいふくした！", "いまはつかえない", UsageType.Always));
        playerGroup.member.AddCommand(new CommandEscape("どうぐ", VocabularyHelper.UseItem, VocabularyHelper.SuccessUseItem, VocabularyHelper.FailUseItem, enemyGroup.member));




        enemyGroup.member.AddCommand(new CommandAttack("こうげき", 5, VocabularyHelper.UseAttack, VocabularyHelper.SuccessEnemyAttack, VocabularyHelper.FailAttack, playerGroup.member, shakeEffect:true));
        // enemyGroup.member.AddCommand(new CommandSpell("ラリホー", 0, 0, 2, "{0}は　ラリホーのじゅもんをとなえた！", "ネタ", "MPがたりない", target: playerGroup.member, spellType: SpellType.Sleep));


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
                    if (playerGroup.member.condition.IsAsleep())
                    {
                        command = playerGroup.member.commands[0];
                        commands.Add(command);
                        ChangePhase(BattlePhase.EnemyChoose);
                        break;
                    }
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
                        ChangePhase(BattlePhase.EnemyChoose);
                    }
                    break;
                case BattlePhase.ChooseSubCommand:
                    windowBattleMagicCommand.Open();
                    CommandMenu commandMenu = command as CommandMenu;
                    windowBattleMagicCommand.Spawn(commandMenu.subs); // 場所が違う

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
    [SerializeField] Transform windowParent;
    IEnumerator ScreenShakeEffect(float duration = 0.1f, bool waitComplete = true)
    {
        Transform parent = windowParent;
        Vector3 targetPos = parent.localPosition;
        int loopCount = 4;
        targetPos.x = Random.Range(2, 4) * (Random.Range(0, 100) > 50 ? -1 : 1);
        targetPos.y = Random.Range(2, 4) * (Random.Range(0, 100) > 50 ? -1 : 1);
        parent.DOLocalMove(targetPos*5, duration).SetLoops(loopCount);
        yield return new WaitForSeconds(waitComplete ? duration * loopCount : 0f);
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
            // 目を覚ましたかどうか
            // ねむっているなら「ねむっている」と表示
            // 目を覚ましたなら、目を覚まして、通常行動
            // ねむっていないなら通常行動
            // 目を覚ました場合, 行動できるようにしたいが

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
                        yield return ScreenShakeEffect();
                    }
                };
                if (command.blinkEffect && command.success) yield return ScreenBlinkEffect();
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
            // PlayerのHPを反映
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

    IEnumerator ScreenBlinkEffect(float duration = 0.5f, bool waitComplete = true)
    {
        Grayscale grayscaleEffect = Camera.main.GetComponent<Grayscale>();
        float blinkFrequency = 0.04f;

        for (float count = 0; count < duration; count += blinkFrequency)
        {
            grayscaleEffect.enabled = !grayscaleEffect.enabled;
            yield return new WaitForSeconds(blinkFrequency);
        }

        grayscaleEffect.enabled = false;
    }
}
