using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 移動が終わったら登録されていることを行う
namespace Players
{
    // ランダムエンカウント
    // もし、モンスターがいる場所を歩いていると、遭遇する
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] Transform movePoint = default;

        [SerializeField] LayerMask whatStopsMovement = default;

        public bool isMoving = default;
        public bool canMove = default;

        public UnityAction EventAction = default;

        void Start()
        {
            movePoint.parent = null;
            canMove = true;
        }

        void Update()
        {
            return;
            transform.position = Vector2.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, movePoint.position) < float.Epsilon)
            {
                if (canMove && Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    Vector3 inputH = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                    MoveTo(inputH);
                }
                else if (canMove && Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    Vector3 inputV = new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
                    MoveTo(inputV);
                }
                else
                {
                    isMoving = false;
                    EventAction?.Invoke();
                }
            }
        }

        public void MoveToDir(Vector3 position)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, movePoint.position) < float.Epsilon)
            {
                if (position == default)
                {
                    isMoving = false;
                    EventAction?.Invoke();
                }
                else
                {
                    MoveTo(position);
                }
            }
        }


        void MoveTo(Vector3 position)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + position * 0.5f, 0.2f, whatStopsMovement))
            {
                isMoving = true;
                movePoint.Translate(position);
            }
        }

        public void SetEncountAction(UnityEvent unityEvent)
        {
            EventAction = unityEvent.Invoke;
            canMove = false;
        }
        public void CancelEncountAction()
        {
            EventAction = null;
            canMove = true;
        }
    }
}