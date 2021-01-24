using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowBattleCommand : WindowBase
{

    [SerializeField] SelectableTextCommand[] selectCommands = default;
    [SerializeField] GameObject textPrefab = default;
    [SerializeField] Transform commandParent = default;

    public int current{ get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        selectCommands = base.windowObj.GetComponentsInChildren<SelectableTextCommand>();
        for (int i = 0; i < selectCommands.Length; i++)
        {
            selectCommands[i].id = i;
            selectCommands[i].SubmitAction = OnOptionSelected;
        }
    }

    public void SetInteractable(bool isActive)
    {
        foreach (SelectableTextCommand selectableTextCommand in selectCommands)
        {
            selectableTextCommand.GetComponent<Selectable>().interactable = isActive;
        }
    }

    void OnOptionSelected(int buttonID)
    {
        current = buttonID;
    }

    public override void Open()
    {
        base.Open();
        SetInteractable(true);
    }
    public void ResetCommandIndex()
    {
        current = 0;
    }
    public void Spawn(List<Command> commandSpells)
    {
        for (int i = 0; i < selectCommands.Length; i++)
        {
            Destroy(selectCommands[i].transform.parent.gameObject);
        }
        selectCommands = new SelectableTextCommand[commandSpells.Count];
        // selectCommands = base.windowObj.GetComponentsInChildren<SelectableTextCommand>();
        for (int i = 0; i < commandSpells.Count; i++)
        {
            SelectableTextCommand selectableTextCommand = Instantiate(textPrefab, commandParent).GetComponentInChildren<SelectableTextCommand>();
            selectableTextCommand.id = i;
            selectableTextCommand.isFirstSelect = i == 0;
            selectableTextCommand.OnActive();
            selectableTextCommand.GetComponent<Text>().text = commandSpells[i].name;
            selectableTextCommand.SubmitAction = OnOptionSelected;
            selectCommands[i] = selectableTextCommand;
        }
        SetNavigation(selectCommands.Length);
    }

    void SetNavigation(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int upIndex = (i - 1 + count) % count;
            int downIndex = (i + 1 + count) % count;
            selectCommands[i].SetNavigation(selectCommands[upIndex], selectCommands[downIndex]);
            selectCommands[i].Init();
        }
    }

    public void ShowCursor()
    {
        EventSystem.current.SetSelectedGameObject(selectCommands[current].gameObject);
    }
}
