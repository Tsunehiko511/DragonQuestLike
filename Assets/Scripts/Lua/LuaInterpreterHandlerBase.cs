using System.Collections;
using UnityEngine;

public abstract class LuaInterpreterHandlerBase : MonoBehaviour
{
    protected bool flag;

    private void Awake()
    {
        flag = true;
    }
    public IEnumerator WaitCoroutine()
    {
        yield return new WaitUntil(() => flag);
    }
}