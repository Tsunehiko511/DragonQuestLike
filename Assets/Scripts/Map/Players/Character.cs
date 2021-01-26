﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name { get; private set; }
    public int strength { get; private set; }
    public int agility { get; private set; }
    int hp;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (hp < 0) hp = 0;
            Debug.Log(value);
        }
    }


    public int mp { get; private set; }
    public int MP
    {
        get
        {
            return mp;
        }
        set
        {
            mp = value;
        }
    }

    public int exp;
    public int level { get; private set; }
    public int gold;
    public bool isPlayer { get; private set; }

    public Command command { get; private set; }

    public List<Command> commands = new List<Command>();

    public Condition condition = new Condition();

    public Character(string name = "char", int strength = 3, int agility = 1, int hp = 10, int mp = 5, int level = 1)
    {
        this.name = name;
        this.strength = strength;
        this.agility = agility;
        this.hp = hp;
        this.mp = mp;
        this.level = level;
        this.exp = 0;
        this.isPlayer = isPlayer;
    }


    public Command AddCommand(Command command)
    {
        commands.Add(command);
        command.SetUser(this);
        return command;
    }


}


public class Enemy : Character
{
    public float dodge = 1f / 64f; //かわす確率
    public static int UNIQUE_ID = 0;
    public int id { get; private set; }

    public Enemy(string name = "char", int strength = 3, int agility = 1, int hp = 10, int mp = 5, int level = 1):base(name, strength, agility, hp, mp, level)
    {
        id = UNIQUE_ID++;
    }

    public float GroupFactor()
    {
        if (id < 19)
        {
            return 0.25f;
        }
        if (id < 29)
        {
            return 0.375f;
        }
        if (id < 34)
        {
            return 0.5f;
        }
        return 1.0f;
    }

}
