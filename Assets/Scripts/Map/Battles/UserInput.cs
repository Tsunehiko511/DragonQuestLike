using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battles
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField] Cursor commandCursor = default;
        [SerializeField] Players.PlayerCore playerCore = default;
        bool canInput;

        private void OnEnable()
        {
            canInput = false;
            Invoke(nameof(Init), 0.3f);
        }

        void Init()
        {
            canInput = true;
        }

        void Update()
        {
            if (canInput == false)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                commandCursor.MoveCursor(Cursor.Direction.Right);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                commandCursor.MoveCursor(Cursor.Direction.Left);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                commandCursor.MoveCursor(Cursor.Direction.Up);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                commandCursor.MoveCursor(Cursor.Direction.Down);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                canInput = false;
                // カーソルがあった時点で実行される
                // 選択中のボタンで決定

                // 選択中のコマンドを取得する
                // Playerに実行コマンドを設定する
                // playerCore.CurrentCommand = 
                // BattleManagerが実行する

                // 技のデータベースを作って、実行になるな
            }
        }
    }
}

