using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    IEnumerator Attack(IDamageable damageable);
}

public interface IDeadable
{
    bool IsDied();
}

public interface IActionable
{
    IEnumerator Attack(IDamageable damageable);
    IEnumerator MagicAction(IDamageable damageable);
    IEnumerator Escape(IDamageable damageable);
    IEnumerator UseTool(IDamageable damageable);
}
