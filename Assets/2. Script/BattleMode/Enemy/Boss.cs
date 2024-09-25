using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace BattleMode
{
    public class Boss : LivingEntity
    {
        protected new Rigidbody2D rigidbody;
        public Slider hpBar;
        public Slider staminaBar;

        [SerializeField]
        protected Transform targetTransform;
        protected Coroutine nowCoroutine;

        protected virtual void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = transform.Find("model").GetComponent<Animator>();
            StartCoroutine(CheckStamina());
        }

        protected virtual void Update()
        {
            if (1 < getState() || getState() == -1)
                return;

            if (targetTransform.position.x > transform.position.x)
                transform.localScale = new Vector3(-4, 4, 1);
            else if (targetTransform.position.x < transform.position.x)
                transform.localScale = new Vector3(4, 4, 1);
        }

        IEnumerator CheckStamina()
        {
            while (true)
            {
                if (stamina <= 0f)
                {
                    setState(-2);
                    //그로기 파티클

                    try
                    {
                        StopCoroutine(nowCoroutine);
                        nowCoroutine = null;
                    }
                    catch (NullReferenceException nre)
                    {
                        Debug.Log(nre);
                    }

                    yield return new WaitForSeconds(2f);
                    Initiate();
                }
                yield return null;
            }
        }

        void Initiate()
        {
            setState(0);
            SetSpeed(5f);
            isAlreadyDamage = true;
            isAlreadyPush = true;
            isGroggyAttack = false;
            nowCoroutine = null;
            ParticleManager.StopRushParticle();

            stamina = 100f;
            staminaBar.value = stamina;
        }

        protected void MoveToPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
        }

        public void AttackTrigger(Collider2D other)
        {
            if (getState() > 1 && !isAlreadyDamage)
            {
                isAlreadyDamage = true;
                other.gameObject.GetComponent<Swordman>().OnDamage(damage);
                if (isGroggyAttack)
                {
                    isGroggyAttack = false;
                    other.gameObject.GetComponent<Swordman>().OnGroggy();
                }
            }
        }
        public void GiveImpulse(Collider2D other)
        {
            if (getState() > 1 && !isAlreadyPush)
            {
                isAlreadyPush = true;
                Vector2 pushVector = new Vector2(6f * -transform.localScale.x / 4, 2f);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(pushVector, ForceMode2D.Impulse);
            }
        }

        public override void OnDamage(float dmg)
        {
            base.OnDamage(dmg);
            SoundManager.EnemyHitSound();
            hpBar.value = health / maxHealth * 100;
        }

        public void OnStiffness(float stiffenPoint)
        {
            stamina -= stiffenPoint;
            staminaBar.value = stamina;
        }

        protected override void Die()
        {
            StopAllCoroutines();
            setState(-1);
            isDead = true;
            GameWinProcess();
        }

        private void GameWinProcess()
        {
            //Sound
            SoundManager.BGMStop();
            SoundManager.PlayGameWinSound();

            //Effect
            StartCoroutine(JumpEffect());
        }

        IEnumerator JumpEffect()
        {
            //time slow 추가
            Time.timeScale = 0.2f;
            Vector2 jumpVector;

            jumpVector = new Vector2(transform.localScale.x / 4 * rigidbody.mass * 10, rigidbody.mass * 8);
            rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);

            Time.timeScale = 1f;

            //Victory UI load
            GameManager.Victory();
            SoundManager.VictoryUIPlay();
        }
    }
}
