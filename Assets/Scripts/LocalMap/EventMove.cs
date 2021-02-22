using System.Collections;
using MoonSharp.Interpreter;
using UnityEngine;

// ランダムエンカウント
// もし、モンスターがいる場所を歩いていると、遭遇する
[MoonSharpUserData]
public abstract class EventMove : LuaInterpreterHandlerBase
{
    const float speed = 7f;

    Vector3Int GetDirection(string direction)
    {
        switch (direction)
        {
            default:
                return Vector3Int.zero;
            case "left":
                return Vector3Int.left;
            case "right":
                return Vector3Int.right;
            case "up":
                return Vector3Int.up;
            case "down":
                return Vector3Int.down;
        }
    }


    public void MoveTo(string direction, int count)
    {
        StartCoroutine(MoveCommand(GetDirection(direction), count));
    }
    public virtual IEnumerator MoveCommand(Vector3Int direction, int coint)
    {
        flag = false;
        yield return null;
        Vector3 target = transform.position + direction * coint;
        while (Vector2.Distance(transform.position, target) > float.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        flag = true;
    }

    public void Wait(float duration)
    {
        StartCoroutine(WaitSecondsCorou(duration));
    }
    IEnumerator WaitSecondsCorou(float duration)
    {
        flag = false;
        yield return new WaitForSeconds(duration);
        flag = true;
    }
}
