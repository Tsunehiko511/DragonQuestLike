using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    IEnumerator Attack(IDamageable damageable, Action<List<string>> messages);
}

public interface IDeadable
{
    bool IsDied();
}

public interface IActionable
{
    IEnumerator Attack(IDamageable damageable, Action<List<string>> messages);
    IEnumerator MagicAction(IDamageable damageable, Action<List<string>> messages);
    IEnumerator Escape(IDamageable damageable, Action<List<string>> messages);
    IEnumerator UseTool(IDamageable damageable, Action<List<string>> messages);
}
