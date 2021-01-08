using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] UnityEvent OnSelectEvent = default;
    // [SerializeField] UnityEvent OnActionEvent = default;
    [SerializeField] GameObject cursorObj = default;
    public void OnSelect()
    {
        cursorObj.SetActive(true);
        // 選択されたとき、特定の関数を渡す
        OnSelectEvent.Invoke();
    }
    public void RemoveSelect()
    {
        cursorObj.SetActive(false);
    }

    // TODO:
    // 選択可能なものにはカーソルがついになっている。
    // 選択状態になったらカーソルがtrueになって、現在の選択コマンドが渡される
}
