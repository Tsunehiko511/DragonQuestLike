using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : MonoBehaviour
{
    [SerializeField] Image monsterImage = default;

    public void SetMoster(Sprite sprite)
    {
        monsterImage.sprite = sprite;
        // 32が200
        Debug.Log(sprite.bounds.size.x);
        monsterImage.rectTransform.sizeDelta = Vector2.one * sprite.bounds.size.x * 200;
    }
}
