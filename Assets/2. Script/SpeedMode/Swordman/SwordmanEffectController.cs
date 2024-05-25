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
        [SerializeField] private ParticleEffect slashHitParticle;
        [SerializeField] private ParticleEffect pierceHitParticle;
        [SerializeField] private ParticleEffect guardParticle;
        [SerializeField] private ParticleEffect healthLossParticle;

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
        }

        public void PlayPierceEffect()
        {
            pierceEffect.Play(effectColor);
        }

        public void PlaySlashHitEffect()
        {
            slashHitParticle.Play();
        }

        public void PlayPierceHitEffect()
        {
            pierceHitParticle.Play();
        }

        public void PlayGuardEffect()
        {
            guardParticle.Play();
        }

        public void PlayHealthLossEffect()
        {
            healthLossParticle.Play();
        }

        public void PlaySkillEffect(Vector3 position)
        {
            position.z = transform.position.z;
            skillSlashEffectPool.GetObject().Play(position, effectColor);
        }
    }
}
