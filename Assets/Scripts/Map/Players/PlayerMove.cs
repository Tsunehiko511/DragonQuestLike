using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Players
{
    // ランダムエンカウント
    // もし、モンスターがいる場所を歩いていると、遭遇する

    public class PlayerMove : MonoBehaviour
    {
        public bool isMoving = default;

        [SerializeField] float speed = default;

        void Start()
        {

        }
        void Update()
        {
            isMoving = false;

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                Move(Vector2.right);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                Move(Vector2.left);
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                Move(Vector2.up);
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                Move(Vector2.down);
            }
        }

        void Move(Vector2 direction)
        {
            isMoving = true;
            transform.position += (Vector3)direction * Time.deltaTime * speed;
        }
    }
}

