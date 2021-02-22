using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HeroCore : MonoBehaviour
{
    [SerializeField] PlayerStatusSO playerStatusSO;

    public UnityAction<Vector3> OnSpace = default;
    [SerializeField] UnityEvent OnUseGold = new UnityEvent();
    [SerializeField] UnityEvent OnGetGold = new UnityEvent();
    [SerializeField] UnityEvent OnHeal = new UnityEvent();

    [System.Serializable]public class InputEvent : UnityEvent<Vector3> { };
    [SerializeField] InputEvent OnInputDirection = new InputEvent();
    bool canInput;
    public bool CanInput
    {
        get => canInput;
        set => canInput = value;
    }

    Vector3 lastDirection = default;
    // 最後の入力
    Vector3 InputDirection
    {
        get
        {
            if (CurrentInputDirection == default)
            {
                return lastDirection;
            }
            return CurrentInputDirection;
        }
    }

    // 現在の入力
    Vector3 CurrentInputDirection
    {
        get
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                return new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                return new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
            }
            return default;
        }
    }

    private void Start()
    {
        canInput = true;

    }

    void Update()
    {
        if (!canInput)
        {
            return;
        }

        if (InputDirection != default)
        {
            lastDirection = InputDirection;
            Debug.Log("CurrentInputDirection:" + CurrentInputDirection);
            OnInputDirection?.Invoke(CurrentInputDirection);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpace?.Invoke(lastDirection);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UseGold(100);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetGold(50);
        }
    }


    // ものを買う:Gold減少
    public void UseGold(int amount)
    {
        playerStatusSO.Gold -= amount;
        OnUseGold?.Invoke();
    }
    // ものを売る:Gold増加
    public void GetGold(int amount)
    {
        playerStatusSO.Gold += amount;
        OnGetGold?.Invoke();
    }

    // 回復:HP上昇
    public void Heal(int amount)
    {
        playerStatusSO.HP += amount;
        OnHeal?.Invoke();
    }

    public void CanInputMethod()
    {
        canInput = true;
    }
}
