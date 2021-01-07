using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battles
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField] Cursor commandCursor = default;
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
        }
    }
}

