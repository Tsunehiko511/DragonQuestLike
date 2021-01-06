using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class PlayerCore : MonoBehaviour
    {
        public Status status;

    }

    public struct Status
    {
        string name;
        int level;
        public int hp;
        int mp;
        int gold;
        int experiencePoint;
        public int at;
    }
}

