﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SelectCommand : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] bool isFirstSelect = default;
    [SerializeField] GameObject cursor = default;
    
    public UnityEvent SubmitEvent = default;

    private void OnEnable()
    {
        Debug.Log("aaa");
        if (isFirstSelect)
        {
            // すぐには移動できない:
            StartCoroutine(CanMoveDelay());
        }
        cursor.SetActive(isFirstSelect);
    }

    IEnumerator CanMoveDelay()
    {
        yield return new WaitForSeconds(0.2f);
        // 選択状態にする
        GetComponent<Selectable>().Select();
    }

    // 選択されたとき
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(gameObject.name);
        cursor.SetActive(true);
    }

    // 選択が外れたとき
    public void OnDeselect(BaseEventData eventData)
    {
        cursor.SetActive(false);
    }

    // スペースが押されたとき
    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log(GetComponent<Text>().text);
        SubmitEvent.Invoke();
    }

}
