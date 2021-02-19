using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStatusSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] new string name = default;
    [SerializeField] int maxHp = default;
    [SerializeField] int hp = default;
    [SerializeField] int level = default;
    [SerializeField] int maxGold = default;
    [SerializeField] int gold = default;

    [NonSerialized] int runtimeHP;
    [NonSerialized] int runtimeGold;

    public string Name
    {
        get => name;
    }

    public int HP
    {
        get => runtimeHP;
        set { runtimeHP = Mathf.Clamp(value, 0, maxHp); }
    }
    public int Level
    {
        get => level;
    }
    public int Gold
    {
        get => runtimeGold;
        set { runtimeGold = Mathf.Clamp(value, 0, maxGold); }
    }

    public void OnAfterDeserialize()
    {
        HP = hp;
        Gold = gold;
    }

    public void OnBeforeSerialize()
    {
    }

    public void DebugLog()
    {
        // ステータス
        Debug.Log(string.Format("Name:{0}, HP:{1}, Gold:{2}", Name, HP, Gold));
    }
}
