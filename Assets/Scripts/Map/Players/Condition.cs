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

// TODO:睡眠の実装&睡眠中は行動できない&睡眠がとけると次のターン行動できるの実装
// 各Characterが持っているもの
public class Condition 
{

    // タイプを入れたら、効果が返ってくる辞書:過去に受けた異常状態が入っている
    public Dictionary<StatusEffectType, StatusEffect> effects;

    // 現在の状態
    StatusEffect effect;
    StatusEffectType effectType;

    public Condition()
    {
        // 初期設定
        effects = new Dictionary<StatusEffectType, StatusEffect>();
    }

    // 効果の追加:ここが始まり:異常状態の重ねがけはされない？？？？　されない
    public void AddEffect(StatusEffectType effectType)
    {
        // TODO:別の異常状態の重ねがけを禁止する
        // ここで効果をつける:睡眠以外もここで振り分ける
        if (effectType == StatusEffectType.Sleep)
        {
            if (effects.ContainsKey(effectType) == false)
            {
                // 初めてその異常状態を受けたら
                // 設定する
                effects.Add(effectType, new StatusEffect(3, 32, false, "{0}は　ねむっている", "{0}は　めをさました"));
            }
            else
            {
                // 2回目以降は前のを使い回し&初期化
                effects[effectType].SetUp();
            }
        }

        // 現在の異常状態を設定する
        effect = effects[effectType];
        this.effectType = effectType;
    }

    // 異常状態の更新:Character characterいらないのでは？
    // public void Update(Character character)
    public void Update()
    {
        if (effect == null) return;

        // 更新する
        effect.Update();
    }

    // チェックとUpdateの違いがわからん
    // Updateは他ターンの経過をする
    // チェックは現在の状態をしる

    // 異常状態中か？ => 更新が入る
    public string Check(Character user)
    {
        if (effect == null) return "";
        string message = "";
        
        if (effect.IsActive())// 継続か？
        {
            message = string.Format(effect.effectMessage, user.name);
        }
        else if (effect.JustEnded())// 終わったか？
        {
            
            message = string.Format(effect.effectEndMessage, user.name);
            effect = null;
            effectType = StatusEffectType.None;
        }
        return message;
    }

    // 行動可能かどうか？(Sleep時はfalseなど?)TODO:どこで使う？
    public bool CanAct()
    {
        if (effect == null) return true; // そもそも異常状態でない

        return effect.canAct;
    }

    // 眠っているかどうか？TODO:どこで使う？
    public bool IsAsleep()
    {
        if (effect == null)
        {
            return false;
        }
        return effectType == StatusEffectType.Sleep;
    }

}

// Condition でつかわえるのみ
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
    // 効果の終了時点
    public string effectEndMessage { get; private set; }

    public StatusEffect(int duration = 3, int healChance = 32, bool canAct = false, string effectMessage = "", string effectEndMessage = "")
    {
        this.duration = duration;
        this.healChance = healChance;
        this.canAct = canAct;
        this.effectMessage = effectMessage;
        this.effectEndMessage = effectEndMessage;
    }

    // 最初に使う
    public void SetUp()
    {
        count = 0;
        state = StatusEffectState.Active;
    }

    // 更新時に使う
    public void Update()
    {
        if (state != StatusEffectState.Active)
        {
            // アクティブじゃないなら何もしない
            return;
        }
        // 完治に成功 && 最初のターンではない:ということは、行動前にUpdateを行う
        bool chanceInac = (Random.Range(0, 100) <= healChance) && count != 0;
        count++;
        Debug.Log("睡眠のターン経過" + count);
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

    // 有効か確かめるために使う
    public bool IsActive()
    {
        return state == StatusEffectState.Active;
    }

    // 終わっているか確かめるために使う
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

