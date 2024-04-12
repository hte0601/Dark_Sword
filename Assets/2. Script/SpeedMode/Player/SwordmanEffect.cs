using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SwordmanEffect : MonoBehaviour
    {
        [SerializeField] private SlashEffect slashEffect;
        [SerializeField] private SlashEffect pierceEffect;

        private SlashEffect.Color effectColor;

        private void Start()
        {
            // 저장된 값을 불러오도록 수정 필요
            effectColor = SlashEffect.Color.Purple;
        }

        public void PlaySlashEffect()
        {
            slashEffect.Play(effectColor);
        }

        public void PlayPierceEffect()
        {
            pierceEffect.Play(effectColor);
        }
    }
}
