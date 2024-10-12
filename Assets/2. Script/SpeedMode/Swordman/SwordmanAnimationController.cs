using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SwordmanAnimationController : MonoBehaviour
    {
        [SerializeField] private SwordmanEffectController effectController;
        private Animator animator;

        private Coroutine nowAnimationCoroutine;
        private int _attackCombo;
        private bool _canComboInput;
        public bool canSpearGoblinCombo;

        public Swordman.State CurrentState
        {
            get => (Swordman.State)animator.GetInteger("State");
            set => animator.SetInteger("State", (int)value);
        }

        public int AttackCombo
        {
            get => _attackCombo;
            private set => _attackCombo = value % 2;
        }

        public bool CanComboInput
        {
            get => _canComboInput;
            private set => _canComboInput = value;
        }


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Initialize()
        {
            AttackCombo = 0;
            CanComboInput = false;
            canSpearGoblinCombo = false;

            CurrentState = Swordman.State.Idle;
        }


        public void RunAttackAnimation(bool isEnemyInRange)
        {
            CurrentState = Swordman.State.Attack;

            if (AttackCombo == 0)
                RunSlashAnimation(isEnemyInRange);
            else
                RunPierceAnimation(isEnemyInRange);

            AttackCombo += 1;
        }

        private void RunSlashAnimation(bool isEnemyInRange)
        {
            animator.CrossFadeInFixedTime("Slash", 0.034f, 0);
            nowAnimationCoroutine = StartCoroutine(SlashAnimation(isEnemyInRange));
        }

        private void RunPierceAnimation(bool isEnemyInRange)
        {
            animator.CrossFadeInFixedTime("Pierce", 0.034f, 0);
            nowAnimationCoroutine = StartCoroutine(PierceAnimation(isEnemyInRange));
        }

        public void RunGuardAnimation(bool isEnemyInRange)
        {
            CurrentState = Swordman.State.Guard;

            animator.CrossFadeInFixedTime("Guard", 0.034f, 0);
            nowAnimationCoroutine = StartCoroutine(GuardAnimation(isEnemyInRange));

            if (canSpearGoblinCombo)
                AttackCombo = 1;
            else
                AttackCombo = 0;
        }

        public void RunSkillAnimation(Action CastSkill)
        {
            CurrentState = Swordman.State.Skill;

            animator.CrossFadeInFixedTime("Skill", 0.05f, 0);
            nowAnimationCoroutine = StartCoroutine(SkillAnimation(CastSkill));

            AttackCombo = 0;
        }

        public void RunGroggyAnimation()
        {
            CurrentState = Swordman.State.Groggy;

            nowAnimationCoroutine = StartCoroutine(GroggyAnimation());
        }

        public void RunDieAnimation()
        {
            CurrentState = Swordman.State.Die;
        }


        public void RunSpearGoblinCombo()
        {
            CanComboInput = false;
            canSpearGoblinCombo = false;
            StopAnimation();

            RunAttackAnimation(true);
        }


        private IEnumerator SlashAnimation(bool isEnemyInRange)
        {
            const int animationLength = 10;

            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (WaitUntilFrame(2, animationLength))
                yield return null;

            effectController.PlaySlashEffect();

            if (isEnemyInRange)
                effectController.PlaySlashHitEffect();

            while (WaitUntilFrame(5, animationLength))
                yield return null;

            CurrentState = Swordman.State.Idle;
        }

        private IEnumerator PierceAnimation(bool isEnemyInRange)
        {
            const int animationLength = 10;

            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (WaitUntilFrame(2, animationLength))
                yield return null;

            effectController.PlayPierceEffect();

            if (isEnemyInRange)
                effectController.PlayPierceHitEffect();

            while (WaitUntilFrame(5, animationLength))
                yield return null;

            CurrentState = Swordman.State.Idle;
        }

        private IEnumerator GuardAnimation(bool isEnemyInRange)
        {
            const int animationLength = 10;

            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (WaitUntilFrame(2, animationLength))
                yield return null;

            if (isEnemyInRange)
                effectController.PlayGuardEffect();

            CanComboInput = true;

            while (WaitUntilFrame(5, animationLength))
                yield return null;

            CanComboInput = false;
            canSpearGoblinCombo = false;
            CurrentState = Swordman.State.Idle;
        }

        private IEnumerator SkillAnimation(Action CastSkill)
        {
            const int animationLength = 60;

            while (!IsAnimation("Skill"))
                yield return null;

            while (WaitUntilFrame(33, animationLength))
                yield return null;

            CastSkill();

            while (IsAnimation("Skill") && !animator.IsInTransition(0))
                yield return null;

            CurrentState = Swordman.State.Idle;
        }

        private IEnumerator GroggyAnimation()
        {
            while (!IsAnimation("Groggy"))
                yield return null;

            while (IsAnimation("Groggy") && !animator.IsInTransition(0))
                yield return null;

            CurrentState = Swordman.State.Idle;
        }


        public void StopAnimation()
        {
            if (nowAnimationCoroutine != null)
                StopCoroutine(nowAnimationCoroutine);
        }

        private bool IsAnimation(string animation, int layerIndex = 0) => animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animation);
        private float NormalizedTime(int layerIndex = 0) => animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
        private float CurrentAnimationFrame(int animationLength, int layerIndex = 0) => animationLength * NormalizedTime(layerIndex);
        private bool WaitUntilFrame(int frame, int animationLength, int layerIndex = 0) => animationLength * NormalizedTime(layerIndex) < frame - 0.25f;
    }
}
