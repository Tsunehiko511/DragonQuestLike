using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ボタン機能をもったテキスト
public class SelectableText : Button, ISelectHandler, IDeselectHandler
{
    // 選択した時の処理
    [SerializeField] ButtonSelectedEvent _onSelect = new ButtonSelectedEvent();
    // 選択が外れたときの処理
    [SerializeField] ButtonDeselectedEvent _onDeselect = new ButtonDeselectedEvent();

    // idを降って
    int id;

    protected SelectableText(){}

    public ButtonSelectedEvent OnSelectText
    {
        get => _onSelect;
        set => _onSelect = value;
    }
    public ButtonDeselectedEvent OnDeselectText
    {
        get => _onDeselect;
        set => _onDeselect = value;
    }

    public int ID
    {
        get => id;
        set => id = value;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        if (!IsActive() || !IsInteractable())
        {
            return;
        }
        _onSelect.Invoke(id);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        if (!IsActive() || !IsInteractable())
        {
            return;
        }
        _onDeselect.Invoke(id);
    }



    [Serializable]
    public class ButtonSelectedEvent : UnityEvent<int> { }
    [Serializable]
    public class ButtonDeselectedEvent : UnityEvent<int> { }
}
