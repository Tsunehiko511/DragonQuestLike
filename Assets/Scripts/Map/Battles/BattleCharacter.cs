using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacter : MonoBehaviour
{
    [SerializeField] Image body;
    // TODO:SE;
    [SerializeField] bool blink;
    const int BLINK_TOTAL = 8;
    const float BLINK_FREQUENCY = 0.05f;

    public void TakeHit()
    {
        StopAllCoroutines();
        // TODO:音をならす
        if (blink)
        {
            StartCoroutine(Blink());
        }
    }

    public void Die()
    {
        body.enabled = false;
    }

    IEnumerator Blink()
    {
        for (int i=0; i< BLINK_TOTAL; i++)
        {
            yield return new WaitForSeconds(BLINK_FREQUENCY);
            body.enabled = !body.enabled;
        }

        body.enabled = true;
        yield break;
    }
}
