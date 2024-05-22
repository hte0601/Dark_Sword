using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SwordmanEffectController : MonoBehaviour
    {
        [Header("Effect")]
        [SerializeField] private AttackSlashEffect slashEffect;
        [SerializeField] private AttackSlashEffect pierceEffect;
        [SerializeField] private ParticleSystem slashHitParticle;
        [SerializeField] private ParticleSystem pierceHitParticle;
        [SerializeField] private ParticleSystem guardParticle;
        [SerializeField] private ParticleSystem healthLossParticle;

        [Header("Skill Effect Prefab")]
        [SerializeField] private SkillSlashEffect skillSlashEffectPrefab;

        private ObjectPool<SkillSlashEffect> skillSlashEffectPool;

        private SlashEffect.Color effectColor;


        private void Awake()
        {
            skillSlashEffectPool = new(skillSlashEffectPrefab, 8, transform);
        }

        private void Start()
        {
            // 저장된 값을 불러오도록 수정 필요
            effectColor = SlashEffect.Color.Purple;
        }


        public void PlaySlashEffect()
        {
            slashEffect.Play(effectColor);
            SoundManager.PlaySFX(SFX.Swordman.Slash);
        }

        public void PlayPierceEffect()
        {
            pierceEffect.Play(effectColor);
            SoundManager.PlaySFX(SFX.Swordman.Slash);
        }

        public void PlaySlashHitEffect()
        {
            slashHitParticle.Play();
            SoundManager.PlaySFX(SFX.Swordman.AttackHit);
        }

        public void PlayPierceHitEffect()
        {
            pierceHitParticle.Play();
            SoundManager.PlaySFX(SFX.Swordman.AttackHit);
        }

        public void PlayGuardEffect()
        {
            guardParticle.Play();
        }

        public void PlayHealthLossEffect()
        {
            healthLossParticle.Play();
            SoundManager.PlaySFX(SFX.Game.HealthLoss);
        }

        public void PlaySkillEffect(Vector3 position)
        {
            position.z = transform.position.z;
            skillSlashEffectPool.GetObject().Play(position, effectColor);
            SoundManager.PlaySFX(SFX.Swordman.Slash);
        }
    }
}
