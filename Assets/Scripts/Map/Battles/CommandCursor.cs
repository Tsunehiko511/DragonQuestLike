using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCursor : MonoBehaviour
{
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    // カーソルの位置を決める
    [SerializeField] Transform[] positionsTransform = default;
    [SerializeField] int row;   // 行
    [SerializeField] int column; // 列
    [SerializeField] float leftPosition; // 横にずらす
    int currentIndex = 0;


    private void OnEnable()
    {
        Move(currentIndex);
    }

    public void MoveCursor(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                currentIndex++;
                if (currentIndex > positionsTransform.Length - 1)
                {
                    currentIndex = 0;
                }

                Move(currentIndex);

                break;
            case Direction.Left:
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = positionsTransform.Length - 1;
                }
                Move(currentIndex);
                break;
            case Direction.Down:
                currentIndex += column;
                if (currentIndex > positionsTransform.Length - 1)
                {
                    currentIndex = currentIndex - (positionsTransform.Length);
                }
                Move(currentIndex);
                break;
            case Direction.Up:
                currentIndex -= column;
                if (currentIndex < 0)
                {
                    currentIndex = positionsTransform.Length + currentIndex;
                }
                Move(currentIndex);
                break;
        }
    }

    // y座標を合わせる？x座標も合わせる？
    private void Move(int index)
    {
        transform.position = positionsTransform[index].position - Vector3.right * leftPosition;
    }
}
