using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCoroutin : MonoBehaviour
{
    public IEnumerator CoroutinEvent()
    {
        yield return null;
    }
}
