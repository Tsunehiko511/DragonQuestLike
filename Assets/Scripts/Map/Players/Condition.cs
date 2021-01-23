using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    None, // オイラ元気！！！
    Sleep,// 睡眠
    Silenced, // 沈黙
    Dead, // 死亡?
}

public class Condition 
{
    public Dictionary<StatusEffectType, StatusEffect> effects;

    // 現在の状態
    StatusEffect effect;
    StatusEffectType effectType;

    public Condition()
    {
        effects = new Dictionary<StatusEffectType, StatusEffect>();
    }

    public void AddEffect(StatusEffectType effectType)
    {
        if (effectType == StatusEffectType.Sleep)
        {
            if (effects.ContainsKey(effectType))
            {
                effects.Add(effectType, new StatusEffect(3, 32, false, "ねむった", "めをさまた"));
            }
            else
            {
                effects[effectType].SetUp();
            }
        }

        effect = effects[effectType];
        this.effectType = effectType;
    }

    public void Update(Character character)
    {
        if (effect == null) return;

        effect.Update();
    }

    // 異常状態中か？
    public string Check(Character user)
    {
        if (effect == null) return "";
        string message = "";
        if (effect.IsActive())
        {
            message = string.Format(effect.effectMessage, user.name);
        }
        else if (effect.JustEnded())
        {
            message = string.Format(effect.effectEndMessage, user.name);
            effect = null;
            effectType = StatusEffectType.None;
        }
        return message;
    }

    // 行動可能かどうか？(Sleep時はfalseなど?)
    public bool CanAct()
    {
        if (effect == null) return true; // そもそも異常状態でない

        return effect.canAct;
    }

    // 眠っている
    public bool IsAsleep()
    {
        return (effect != null && effectType == StatusEffectType.Sleep);
    }

}


public class StatusEffect
{
    int count;      // 効果時間の計測
    int duration;   // 最長?
    int healChance; // 回復率

    // 行動可能かどうか？(Sleep時はfalse)
    public bool canAct { get; private set; }

    enum StatusEffectState
    {
        Active,
        JustEnded,
        InActive,
    }

    StatusEffectState state;

    public string effectMessage { get; private set; }
    public string effectEndMessage { get; private set; }

    public StatusEffect(int duration = 3, int healChance = 32, bool canAct = false, string effectMessage = "", string effectEndMessage = "")
    {
        this.duration = duration;
        this.healChance = healChance;
        this.canAct = canAct;
        this.effectMessage = effectMessage;
        this.effectEndMessage = effectEndMessage;
    }


    public void SetUp()
    {
        count = 0;
        state = StatusEffectState.Active;
    }

    // 
    public void Update()
    {
        if (state != StatusEffectState.Active)
        {
            // アクティブじゃないなら何もしない
            return;
        }
        // 完治に成功 && 1ターン以上経過
        bool chanceInac = (Random.Range(0, 100) <= healChance) && count != 0;
        count++;
        if ((count > duration) || chanceInac)
        {
            // 完治
            state = StatusEffectState.JustEnded;
        }
        else
        {
            // まだまだ継続
            state = StatusEffectState.Active;
        }
    }

    public bool IsActive()
    {
        return state == StatusEffectState.Active;
    }

    public bool JustEnded()
    {
        bool result = (state == StatusEffectState.JustEnded);
        if (result)
        {
            state = StatusEffectState.InActive;
        }
        return result;
    }
}
