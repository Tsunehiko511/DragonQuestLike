using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ScreenEffect : MonoBehaviour
{
    [SerializeField] Transform windowParent = default;
    [SerializeField] BattleManager battleManager = default;

    float duration = 0.5f;
    private void Awake()
    {
        battleManager.OnScreenEffectEvent += BlinkEffect;
        battleManager.OnPlayerTakeDamage += ShakeEffect;
    }



    public IEnumerator BlinkEffect()
    {
        Grayscale grayscaleEffect = Camera.main.GetComponent<Grayscale>();
        float blinkFrequency = 0.04f;

        for (float count = 0; count < duration; count += blinkFrequency)
        {
            grayscaleEffect.enabled = !grayscaleEffect.enabled;
            yield return new WaitForSeconds(blinkFrequency);
        }

        grayscaleEffect.enabled = false;
    }


    public IEnumerator ShakeEffect()
    {
        float duration = 0.1f;
        bool waitComplete = true;
        Transform parent = windowParent;
        Vector3 targetPos = parent.localPosition;
        int loopCount = 4;
        targetPos.x = Random.Range(2, 4) * (Random.Range(0, 100) > 50 ? -1 : 1);
        targetPos.y = Random.Range(2, 4) * (Random.Range(0, 100) > 50 ? -1 : 1);
        parent.DOLocalMove(targetPos * 5, duration).SetLoops(loopCount);
        yield return new WaitForSeconds(waitComplete ? duration * loopCount : 0f);
    }


}
