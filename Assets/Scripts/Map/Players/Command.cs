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

public class CommandAttack : Command
{
    public int baseDamage { get; private set; }

    public CommandAttack(string name = "こうげき", int baseDamage = 0, string useMessage = "", string successMessage = "", string failMessage = "", Character target = null) : base(name, useMessage, successMessage, failMessage, target)
    {
        this.baseDamage = baseDamage;
    }
    public override void Execute()
    {
        base.Execute();

        int targetDefense = (int)Mathf.Floor(target.agility / 2);
        int min, max, damage = baseDamage;
        if (user is Enemy)
        {
            if (user.strength > targetDefense)
            {
            }
            else
            {
                min = 0;
            }
        }
        else
        {
            Debug.Log("playerの攻撃");
        }
        target.HP -= damage;
        useMessage = string.Format("{0}の　こうげき", user.name);
        resultMessage = string.Format("{0}は　{1}ポイントの\nダメージをうけた", target.name, damage);
        success = damage > 0;
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
    string excuteMessage;// useMessageと同じ
    public override void Execute()
    {
        // MPがたりない
        if (CanCast() == false)
        {
            excuteMessage = "";
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
            damage = (int)Mathf.Round(damage);
            target.HP -= damage;
            resultMessage = string.Format(successMessage, target.name, damage);
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