using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    /*
    public enum Type
    {
        Attack,
        Escape,
    }*/

    public Character target { get; private set; }
    public string name { get; private set; }
    public bool success { get; set; }
    public Character user { get; private set; }
    public string useMessage { get; set; }
    public string failMessage { get; set; }
    public string successMessage { get; set; }
    public string resultMessage { get; set; }
    public Command(string name = "こうげき", string useMessage = "", string successMessage = "", string failMessage = "", Character target = null)
    {
        this.name = name;
        this.useMessage = useMessage;
        this.failMessage = failMessage;
        this.successMessage = successMessage;
        this.target = target;
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

    public CommandAttack(string name = "こうげき", int baseDamage = 0, string useMessage = "", string successMessage = "", string failMessage = "", Character target = null) : base(name, useMessage, successMessage, failMessage, target)
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
                if (Random.Range(0, 100) > 50)
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
                if (Random.Range(0, 100) > 50)
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
        useMessage = string.Format("{0}の　こうげき", user.name);
        successMessage = string.Format("{0}は　{1}ポイントの\nダメージをうけた", target.name, damage);
        failMessage = string.Format("{0}は　ひらりとかわした", target.name);
        success = damage > 0;
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

public class CommandEscape : Command
{
    public CommandEscape(string name = "にげる", string useMessage = "", string successMessage = "", string failMessage = "", Character target = null) : base(name, useMessage, successMessage, failMessage, target)
    { 
    }

    public override void Execute()
    {
        useMessage = string.Format("{0}は　にげるをせんたく", user.name);
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
    // エフェクト
    public CommandSpell(string name = "",　int baseDamage = 0, int maxDamage = 0, int mpCost = 0, string useMessage = "", string successMessage = "", string failMessage = "", Character target = null, SpellType spellType = SpellType.Normal) : base(name, baseDamage, useMessage, successMessage, failMessage, target)
    {
        this.maxDamage = maxDamage;
        this.mpCost = mpCost;
        this.type = spellType;
        if (type == SpellType.Sleep)
        {
            // エフェクトをスリープにする
        }
    }

    // TODO:何者？
    public bool excuted = true;
    public override void Execute()
    {
        // MPがたりない
        if (CanCast() == false)
        {
            resultMessage = "MPがたりない！";
            excuted = false;
            return;
        }

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
            // 
        }
        else
        {
            resultMessage = failMessage;
        }
        excuted = true;
    }

    bool CanCast()
    {
        return mpCost < user.mp;
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