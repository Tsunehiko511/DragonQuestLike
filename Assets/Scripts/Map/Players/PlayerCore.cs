
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// メッセージをどうやって出すのか？
// システムの方で処理を終わらせておいて、描画していく？

namespace Players
{
    public class PlayerCore : MonoBehaviour, IDamageable, IAttackable, IDeadable
    {
        new StringReactiveProperty name = new StringReactiveProperty("ゆうしゃ");
        IntReactiveProperty level = new IntReactiveProperty(1);
        IntReactiveProperty hp = new IntReactiveProperty(1);
        IntReactiveProperty mp = new IntReactiveProperty(1);
        IntReactiveProperty gold = new IntReactiveProperty(1);
        IntReactiveProperty experiencePoint = new IntReactiveProperty(1);
        IntReactiveProperty at = new IntReactiveProperty(1);

        public delegate IEnumerator CurrentCommandDelegate();
        public CurrentCommandDelegate CurrentCommand;

        public int AT
        {
            get => at.Value;
        }
        public int HP
        {
            get { return hp.Value; }
            set
            {
                hp.Value = value;
                if (hp.Value < 0) hp.Value = 0;
            }
        }

        public int Speed { get; set; } = 10;


        public void Damage(int damage)
        {
            HP -= damage;
        }

        public IEnumerator Attack(IDamageable damageable)
        {
            damageable.Damage(AT);
            return null;
        }

        public void MagicAction()
        {
        }
        public void Escape()
        {
        }
        public void UseTool()
        {
        }

        public bool IsDied()
        {
            return HP <= 0;
        }
    }

    public struct Status
    {
        string name;
        int level;
        public int hp;
        int mp;
        int gold;
        int experiencePoint;
        public int at;
    }
}

