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