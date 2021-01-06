using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagers
{
    public enum GameMode
    {
        Title,
        Map,
        Battle,
    }

    public class GameManager : MonoBehaviour
    {
        public GameMode gameMode;
        public static GameManager instance;
        private void Awake()
        {
            instance = this;
        }
    }
}
