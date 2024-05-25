using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class AttackSlashEffect : SlashEffect
    {
        public void Play(Color effectColor = Color.Red)
        {
            PlaySlashAnimation(effectColor);
            PlaySoundEffect();
        }
    }
}
