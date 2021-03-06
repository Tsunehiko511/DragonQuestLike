﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public Character target { get; private set; }
    public string name { get; private set; }
    public bool success { get; set; }
    public Character user { get; private set; }
    public string useMessage { get; set; }
    public string failMessage { get; set; }
    public string successMessage { get; set; }
    public string resultMessage { get; set; }
    public bool shakeEffect;
    public bool blinkEffect;

    public Command(string name = "こうげき", string useMessage = "", string successMessage = "", string failMessage = "", Character target = null, bool shakeEffect = false, bool blinkEffect = false)
    {
        this.name = name;
        this.useMessage = useMessage;
        this.failMessage = failMessage;
        this.successMessage = successMessage;
        this.target = target;
        this.shakeEffect = shakeEffect;
        this.blinkEffect = blinkEffect;
    }

    public void SetUser(Character user)
    {
        this.user = user;
    }


    public virtual void Execute()
    {
    }

    protected int RandomNum()
    {
        return Random.Range(0, 255);
    }
}

// 通常攻撃
public class CommandAttack : Command
{
    public int baseDamage { get; private set; }

    public CommandAttack(string name = "こうげき", int baseDamage = 0, string useMessage = "", string successMessage = "", string failMessage = "", Character target = null, bool shakeEffect = false, bool blinkEffect = false) : base(name, useMessage, successMessage, failMessage, target, shakeEffect, blinkEffect)
    {
        this.baseDamage = baseDamage;
    }
    // TODO:装備（攻撃力 = ちから+武器の攻撃）
    public override void Execute()
    {
        base.Execute();

        int targetDefense = (int)Mathf.Floor(target.agility / 2); // + 装備
        int min, max, damage = 0;
        if (user is Enemy)
        {
            if (user.strength > targetDefense)
            {
                min = (user.strength - targetDefense / 2) / 4;
                max = (user.strength - targetDefense / 2) / 2;
                damage = Random.Range(min, max + 1);
            }
            else
            {
                // 勇者の防御が強い場合
                Debug.Log("修正:勇者の防御が強い場合");
                if (Random.Range(0, 100) > 50 || target.condition.IsAsleep())
                {
                    damage = 1;
                }
                else
                {
                    damage = 0;
                }
            }
        }
        else
        {
            // 勇者の攻撃
            min = (user.strength - targetDefense / 2) / 4;
            max = (user.strength - targetDefense / 2) / 2;
            damage = Random.Range(min, max + 1);
            if (damage < 1)
            {
                if (Random.Range(0, 100) > 50 || target.condition.IsAsleep())
                {
                    damage = 1;
                }
                else
                {
                    damage = 0;
                }
            }
        }
        target.HP -= damage;
        useMessage = string.Format(useMessage, user.name);
        success = damage > 0;
        if (success)
        {
            resultMessage = string.Format(successMessage, target.name, damage);
        }
        else
        {
            resultMessage = string.Format(failMessage, target.name, name);
        }
    }
}

public class CommandEscape : Command
{
    public CommandEscape(string name = "にげる", string useMessage = "", string successMessage = "", string failMessage = "", Character target = null) : base(name, useMessage, successMessage, failMessage, target)
    { 
    }

    public override void Execute()
    {
        useMessage = string.Format(useMessage, user.name);
        // 逃げるを選択
        // メッサージ
        if ((user is Enemy) == false)
        {
            success = user.agility * RandomNum() > target.agility * RandomNum() * (target as Enemy).GroupFactor();
        }

        if (success)
        {
            resultMessage = successMessage;
        }
        else
        {
            resultMessage = failMessage;
        }
    }

}

public enum SpellType
{
    Normal,
    Heal,   // かいふく
    Hurt,   // ダメージを与える
    Sleep,  // 眠らせる
}

public class CommandSpell: CommandAttack
{
    public int maxDamage { get; protected set; }
    public int mpCost { get; protected set; }
    public SpellType type { get; protected set; }

    public StatusEffectType effect;

    // エフェクト
    public CommandSpell(string name = "",　int baseDamage = 0, int maxDamage = 0, int mpCost = 0, string useMessage = "", string successMessage = "", string failMessage = "", Character target = null, SpellType spellType = SpellType.Normal, bool shakeEffect = false, bool blinkEffect = false) : base(name, baseDamage, useMessage, successMessage, failMessage, target, shakeEffect, blinkEffect)
    {
        this.maxDamage = maxDamage;
        this.mpCost = mpCost;
        this.type = spellType;
        if (type == SpellType.Sleep)
        {
            // エフェクトをスリープにする
            effect = StatusEffectType.Sleep;
        }
    }

    // 再度実行するためのもの

    public override void Execute()
    {
        useMessage = string.Format(useMessage, user.name, name);

        user.MP -= mpCost;
        int resist = GetTargetResistance();
        success = resist < Random.Range(0, 100);
        if (success)
        {
            int damage = Random.Range(baseDamage, maxDamage);
            damage = Mathf.RoundToInt(damage);
            target.HP -= damage;
            resultMessage = string.Format(successMessage, target.name, Mathf.Abs(damage));

            // 異常状態の効果をもってるなら適応
            if (effect != StatusEffectType.None)
            {
                target.condition.AddEffect(effect);
            }
        }
        else
        {
            resultMessage = failMessage;
        }
    }

    bool CanCast()
    {
        if (user is Enemy)
        {
            return true;
        }
        return mpCost < user.mp;
    }

    public bool CanExecute()
    {
        // MPがたりない
        if (CanCast())
        {
            return true;
        }
        resultMessage = VocabularyHelper.NotEnoughMP;
        return false;

    }



    // 呪文に対する抵抗値？かかりやすさ？
    int GetTargetResistance()
    {
        return 0;
    }
}

public class CommandMenu : Command
{
    public List<Command> subs { get; protected set; }

    // 生成時に代入される
    public CommandMenu(string name = "", string useMessage = "", string successMessage = "", string failMessage = "") : base(name, useMessage, successMessage, failMessage)
    {
    }

    public override void Execute()
    {
        success = HasSubCommands();
        if (success)
        {
            resultMessage = successMessage;
        }
        else
        {
            resultMessage = failMessage;
        }
    }

    public Command AddSub(Command command)
    {
        if (subs == null) subs = new List<Command>();
        command.SetUser(user);
        subs.Add(command);
        return command;
    }

    public bool HasSubCommands()
    {
        return subs != null && subs.Count > 0;
    }
}


// アイテムの実装
public enum UsageType
{
    Never,      // 何度でも?使えない？
    Always,     // バトルとフィールドで使える
    BattleOnly,
    FieldOnly,
}


// 呪文と同じように使う
public class CommandItem : CommandAttack
{

    public UsageType usage { get; private set; }

    public CommandItem(string name = "", string useMessage = "", string successMessage = "", string failMessage = "", UsageType usage = UsageType.Always, Character target = null, int baseDamage = 0) : base(name, baseDamage, useMessage, successMessage, failMessage, target)
    {
        this.usage = usage;
    }

    public override void Execute()
    {
        resultMessage = string.Format(successMessage, 20);
    }

    public bool CanUseInBattle()
    {
        if(usage == UsageType.Always || usage == UsageType.BattleOnly)
        {
            useMessage = string.Format(useMessage, user.name, name);
            return true;
        }
        resultMessage = failMessage;
        return false;
    }
}