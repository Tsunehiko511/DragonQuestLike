using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battles
{
    public enum Commands
    {
        Attack,
        Magic,
        Escape,
        UseTool,
    }

    [Serializable]
    public class BattlerBase : IDamageable, IDeadable, IActionable
    {
        [Serializable]
        public struct Status
        {
            public string name;
            public int level;
            public int hp;
            public int mp;
            public int at;
            public int speed;
            public int ex;
            public int gold;
        }

        public BattlerBase(BattlerBase battlerBase, bool isPlayer = false)
        {
            status.name = battlerBase.Name;
            status.hp = battlerBase.HP;
            status.at = battlerBase.AT;
            status.speed = battlerBase.Speed;
            status.ex = battlerBase.Ex;
            status.gold = battlerBase.Gold;
            this.isPlayer = isPlayer;
        }

        [SerializeField] bool isPlayer;

        public delegate IEnumerator Command(IDamageable damageable, Action<List<string>> message);
        public Command selectCommand = default;
        public Command SelectCommand
        {
            get
            {
                if (selectCommand == null)
                {
                    return Attack;
                }
                return selectCommand;
            }
        }

        public bool IsPlayer
        {
            get => isPlayer;
        }


        public string Name
        {
            get => status.name;
            set => status.name = value;

        }
        public int Level
        {
            get => status.level;
            set => status.level = value;

        }

        public int HP
        {
            get => status.hp;
            set
            {
                status.hp = value;
                if (status.hp <= 0)
                {
                    status.hp = 0;
                }
            }
        }
        public int MP
        {
            get => status.mp;
            set
            {
                status.mp = value;
                if (status.mp <= 0)
                {
                    status.mp = 0;
                }
            }
        }
        public int AT
        {
            get => status.at;
            set => status.at = value;
        }


        public int Speed
        {
            get => status.speed;
            set => status.speed = value;

        }
        public int Ex
        {
            get => status.ex;
            set => status.ex = value;

        }
        public int Gold
        {
            get => status.gold;
            set => status.gold = value;

        }

        public Status status = new Status();

        public IEnumerator Damage(int damage, Action<List<string>> message)
        {
            HP -= damage;
            if (isPlayer)
            {
                message(new List<string>
                {
                    Name + "は　" + damage + "ポイントの",
                    "ダメージを　うけた!"
                });
            }
            else
            {
                message(new List<string>
                {
                    Name + "に　" + damage + "ポイントの",
                    "ダメージを　あたえた!"
                });
            }
            return null;
        }

        public IEnumerator Attack(IDamageable damageable, Action<List<string>> messages)
        {
            List<string> resultMessage = new List<string>()
            {
                Name + "の　こうげき！"
            };
            yield return damageable.Damage(status.at, r => resultMessage.AddRange(r));
            messages(resultMessage);
        }

        public bool IsDied()
        {
            return HP <= 0;
        }

        public IEnumerator MagicAction(IDamageable damageable, Action<List<string>> messages)
        {
            Debug.Log("じゅもん");
            yield return null;
        }

        public IEnumerator Escape(IDamageable damageable, Action<List<string>> messages)
        {
            Debug.Log("逃げる");
            yield return null;
        }

        public IEnumerator UseTool(IDamageable damageable, Action<List<string>> messages)
        {
            Debug.Log("どうぐ");
            yield return null;
        }

        public void SetCommand(Commands command)
        {
            switch(command)
            {
                default:
                case Commands.Attack:
                    selectCommand = Attack;
                    break;
                case Commands.Magic:
                    selectCommand = MagicAction;
                    break;
                case Commands.Escape:
                    selectCommand = Escape;
                    break;
                case Commands.UseTool:
                    selectCommand = UseTool;
                    break;
            }
        }
    }

    // わざごとにエフェクトがある
}
