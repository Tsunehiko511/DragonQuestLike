using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 単語の変換のみに使うもの
public static class VocabularyHelper
{
    public static string BattleStart(string enemy)
    {
        return string.Format("{0} があらわれた！", enemy);
    }

    public static string PlayerAttack(string enemy, int amount)
    {
        return string.Format("{0}に　{1}ポイントの\nダメージをあたえた！", enemy, amount);
    }
    public static string EnemyAttack(string player, int amount)
    {
        return string.Format("　{0}は　{1}ポイントの\n　ダメージをうけた！", player, amount);
    }


    public static string Victory(string enemy)
    {
        return string.Format("{0} をたおした", enemy);
    }
    public static string Gold(int amount)
    {
        return string.Format("{0}ゴールドを　てにいれた！", amount);
    }
    public static string Exp(int amount)
    {
        return string.Format("けいけんち　{0}ポイントかくとく", amount);
    }

}
