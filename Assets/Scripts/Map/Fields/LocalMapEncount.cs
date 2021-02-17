using UnityEngine;
using UnityEngine.Events;
using Players;

namespace Maps
{
    public class LocalMapEncount : FieldBase
    {
        [SerializeField] UnityEvent EncountEvent = default;
        // アクションが終わったら実行したいことも登録する?

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") == false)
            {
                return;
            }
            PlayerMove player = collision.GetComponent<PlayerMove>();
            player.SetEncountAction(EncountEvent);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") == false)
            {
                return;
            }
            if (transform.parent.gameObject.activeSelf == false)
            {
                return;
            }
            PlayerMove player = collision.GetComponent<PlayerMove>();
            player.CancelEncountAction();
        }
    }
}

