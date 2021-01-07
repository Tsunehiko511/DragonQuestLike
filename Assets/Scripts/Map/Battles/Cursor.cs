using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    Text[] cursorObjs;

    // カーソルの位置を決める
    [SerializeField] int row;   // 行
    [SerializeField] int column; // 列
    int currentIndex = 0;
    void Awake()
    {
        cursorObjs = GetComponentsInChildren<Text>();
    }

    private void OnEnable()
    {
        Show(currentIndex);
    }
    public void MoveCursor(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                currentIndex++;
                if (currentIndex > cursorObjs.Length - 1)
                {
                    currentIndex = 0;
                }

                Show(currentIndex);

                break;
            case Direction.Left:
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = cursorObjs.Length - 1;
                }
                Show(currentIndex);
                break;
            case Direction.Down:
                currentIndex += column;
                if (currentIndex > cursorObjs.Length - 1)
                {
                    currentIndex = currentIndex - (cursorObjs.Length);
                }
                Show(currentIndex);
                break;
            case Direction.Up:
                currentIndex -= column;
                if (currentIndex < 0)
                {
                    currentIndex = cursorObjs.Length + currentIndex;
                }
                Show(currentIndex);
                break;
        }
    }

    // y座標を合わせる？x座標も合わせる？
    private void Show(int index)
    {
        foreach (Text text in cursorObjs)
        {
            text.gameObject.SetActive(false);
        }
        cursorObjs[index].gameObject.SetActive(true);
    }

}
