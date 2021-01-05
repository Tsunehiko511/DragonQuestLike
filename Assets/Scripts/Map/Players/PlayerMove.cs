using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    // ランダムエンカウント
    // もし、モンスターがいる場所を歩いていると、遭遇する

    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] float speed = default;
        void Start()
        {

        }
        void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Move(Vector2.right);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Move(Vector2.left);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Move(Vector2.up);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Move(Vector2.down);
            }
        }

        void Move(Vector2 direction)
        {
            transform.position += (Vector3)direction*Time.deltaTime*speed;
        }
    }
}

