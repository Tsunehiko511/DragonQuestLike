using System.Collections;
using MoonSharp.Interpreter;
using UnityEngine;

// ランダムエンカウント
// もし、モンスターがいる場所を歩いていると、遭遇する
[MoonSharpUserData]
public class TownDoor : LuaInterpreterHandlerBase
{
    public void Open()
    {
        gameObject.SetActive(false);
    }
}
