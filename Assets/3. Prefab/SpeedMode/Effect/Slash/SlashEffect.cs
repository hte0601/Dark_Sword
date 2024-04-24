using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SlashEffect : MonoBehaviour
    {
        public enum Color
        {
            Red,
            Green,
            Blue,
            Purple
        }

        [SerializeField] protected Animator animator;
    }
}
