using System.Collections;
using UnityEngine;
using Players;

public class GameEventManager : MonoBehaviour
{
    // [SerializeField] EventData eventData = default;
    [SerializeField] PlayerMove player = default;
    [SerializeField] EventMove npc = default;
    [SerializeField] TownDoor door = default;
    [SerializeField] LocalMessagePanel messagePanel = default;
    [SerializeField] TextAsset textAsset = default;

    private void Start()
    {
        StartCoroutine(ExecuteEvent(textAsset.text));
    }

    IEnumerator ExecuteEvent(string script)
    {
        var interpreter = new LuaInterpreter(script); // スクリプトを渡して初期化
        interpreter.AddHandler("player", player); // メッセージ制御のハンドラを登録
        interpreter.AddHandler("npc", npc); // メッセージ制御のハンドラを登録
        interpreter.AddHandler("message", messagePanel); // メッセージ制御のハンドラを登録
        interpreter.AddHandler("door", door); // メッセージ制御のハンドラを登録
        yield return null;
        while (interpreter.HasNextScript())
        {
            interpreter.Resume();
            yield return interpreter.WaitCoroutine();
        }
    }
}