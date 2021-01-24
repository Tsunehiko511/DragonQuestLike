
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
        return string.Format("{0}は　{1}ポイントの\n　ダメージをうけた！", player, amount);
    }

    // こうげき
    public const string UseAttack = "{0}の　こうげき！";

    public const string SuccessPlayerAttack = "{0}に {1}ポイントの\nダメージをあたえた！";
    public const string SuccessEnemyAttack =  "{0}は {1}ポイントの\nダメージをうけた！";

    public const string FailAttack = "しかし　{0}は\n{1}を かわした！";

    // 呪文
    public const string Heal = "{0}は {1}ポイント\nかいふくした！";
    public const string NotEnoughMP = "MPが足りない";
    public const string UseSpell = "{0}は　{1}をとなえた！";
    public const string SuccessSleepSpell = "{0}は　ねむってしまった！";


    public const string FailSleepSpell = "{0}には\nきかなかった！";

    // 逃げる
    public const string UseEscape = "{0}は　にげだした！";
    public const string SuccessEscape = "";
    public const string FailEscape = "しかし　まわりこまれてしまった！";

    // 道具
    public const string UseItem = "{0}は　{1}をつかった！";
    public const string SuccessUseItem = "";
    public const string FailUseItem = "しかし　なにもおこらない";

    // 終了メッセージ
    public static string Victory(string enemy)
    {
        return string.Format("{0} をたおした！", enemy);
    }

    public static string Gold(int amount)
    {
        return string.Format("{0}ゴールドを　てにいれた！", amount);
    }
    public static string Exp(int amount)
    {
        return string.Format("けいけんち　{0}ポイントかくとく！", amount);
    }

    public const string Command = "コマンド？";
}
