using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
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
            public StringReactiveProperty name;
            public IntReactiveProperty level;
            public IntReactiveProperty hp;
            public IntReactiveProperty at;
            public IntReactiveProperty speed;
        }

        public BattlerBase(BattlerBase battlerBase, bool isPlayer = false)
        {
            status.name = new StringReactiveProperty(battlerBase.Name);
            status.hp = new IntReactiveProperty(battlerBase.HP);
            status.at = new IntReactiveProperty(battlerBase.AT);
            status.speed = new IntReactiveProperty(battlerBase.Speed);
            this.isPlayer = isPlayer;
        }

        [SerializeField] bool isPlayer;

        public delegate IEnumerator Command(IDamageable damageable);
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
            get => status.name.Value;
            set => status.name.Value = value;

        }
        public int HP
        {
            get => status.hp.Value;
            set
            {
                status.hp.Value = value;
                if (status.hp.Value <= 0)
                {
                    status.hp.Value = 0;
                }
            }
        }
        public int AT
        {
            get => status.at.Value;
            set => status.at.Value = value;
        }


        public int Speed
        {
            get => status.speed.Value;
            set => status.speed.Value = value;

        }

        public Status status = new Status();

        public IEnumerator Damage(int damage)
        {
            Debug.Log(Name + "は" + damage + "をうけた");
            HP -= damage;
            return null;
            // yield return new WaitForSeconds(0.3f);
        }

        public IEnumerator Attack(IDamageable damageable)
        {
            Debug.Log(Name + "のこうげき");
            yield return damageable.Damage(status.at.Value);
            // yield return new WaitForSeconds(0.3f);
        }

        public bool IsDied()
        {
            return HP <= 0;
        }

        public IEnumerator MagicAction(IDamageable damageable)
        {
            Debug.Log("じゅもん");
            yield return null;
        }

        public IEnumerator Escape(IDamageable damageable)
        {
            Debug.Log("逃げる");
            yield return null;
        }

        public IEnumerator UseTool(IDamageable damageable)
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
}
