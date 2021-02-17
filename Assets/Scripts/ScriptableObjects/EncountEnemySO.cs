using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemys;

[CreateAssetMenu]
public class EncountEnemySO : ScriptableObject
{
    [SerializeField] EnemyCore enemyCore = default;

    public EnemyCore EncountEnemy
    {
        get => enemyCore;
        set => enemyCore = value;
    }
}
