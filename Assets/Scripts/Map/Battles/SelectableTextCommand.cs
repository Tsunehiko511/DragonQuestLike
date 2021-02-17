using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SelectableTextCommand : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public bool isFirstSelect = default;
    [SerializeField] GameObject cursor = default;

    public int id;

    public UnityAction<int> SubmitAction = default;

    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {
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
        Debug.Log("選択!!!");
        GetComponent<Selectable>().Select();

    }

    // 選択されたとき
    public void OnSelect(BaseEventData eventData)
    {
        cursor.SetActive(true);
        isFirstSelect = true;
        SubmitAction.Invoke(id);
    }

    // 選択が外れたとき
    public void OnDeselect(BaseEventData eventData)
    {
        cursor.SetActive(false);
        isFirstSelect = false;
        SoundManager.instance.PlaySE(SoundManager.SE.Button);
    }

    // スペースが押されたとき
    public void OnSubmit(BaseEventData eventData)
    {
        SubmitAction.Invoke(id);
        SoundManager.instance.PlaySE(SoundManager.SE.Button);
    }

    public void OnActive()
    {
        cursor.SetActive(isFirstSelect);
    }

    public void SetNavigation(SelectableTextCommand up, SelectableTextCommand down)
    {
        Selectable select = GetComponent<Selectable>();
        Navigation navigation = select.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnUp = up.GetComponent<Selectable>();
        navigation.selectOnDown = down.GetComponent<Selectable>();
        select.navigation = navigation;
    }
}
