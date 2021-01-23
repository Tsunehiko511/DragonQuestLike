using UnityEngine;

public class Group
{
    public Character member { get; private set; }

    public Character AddCharacter(string name = "char", int hp = 10, int mp = 5, int level = 1)
    {
        Character character = new Character(name, hp: hp, mp: mp, level: level);
        member = character;
        return character;
    }
    public Enemy AddEnemy(string name = "char", int hp = 10, int mp = 5, int level = 1)
    {
        Enemy enemy = new Enemy(name: name, hp: hp, mp: mp, level: level);
        member = enemy;
        return member as Enemy;
    }

    public bool Dead()
    {
        return member.HP == 0;
    }
}
