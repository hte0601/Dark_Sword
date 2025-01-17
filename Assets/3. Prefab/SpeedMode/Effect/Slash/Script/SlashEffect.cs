using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SlashEffect : AnimationEffect
    {
        public enum Color
        {
            Red,
            Green,
            Blue,
            Purple
        }

        protected void PlaySlashAnimation(Color effectColor)
        {
            if (effectColor == Color.Red)
                animator.Play("Red_Slash");
            else if (effectColor == Color.Green)
                animator.Play("Green_Slash");
            else if (effectColor == Color.Blue)
                animator.Play("Blue_Slash");
            else if (effectColor == Color.Purple)
                animator.Play("Purple_Slash");
        }
    }
}
