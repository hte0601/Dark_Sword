using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class Swordman : MonoBehaviour
    {
        public enum State
        {
            Die = -2,
            Groggy = -1,
            Idle = 0,
            Attack = 1,
            Guard = 2,
            Skill = 3,
        }

        public static Swordman instance;
        private EnemyManager enemyManager;
        private Animator animator;

        private float BATTLE_RANGE;

        private int attackCombo = 1;
        private Coroutine nowCoroutine;

        public int AttackCombo
        {
            get => attackCombo;
            private set => attackCombo = value;
        }

        public State CurrentState
        {
            get => (State)animator.GetInteger("State");
            private set => animator.SetInteger("State", (int)value);
        }


        private void Awake()
        {
            instance = this;
            animator = transform.Find("model").GetComponent<Animator>();

            BATTLE_RANGE = transform.position.x + ModeData.SwordmanData.MAX_BATTLE_RANGE;
        }

        private void Start()
        {
            enemyManager = EnemyManager.instance;
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKey(KeyCode.A))
                    HandleInput(State.Attack);

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.L))
                    HandleInput(State.Guard);

                if (Input.GetKey(KeyCode.D))
                    HandleInput(State.Skill);
            }
        }

        // 수정 필요
        // for moblie
        public void Attack()
        {
            HandleInput(State.Attack);
        }

        public void Defense()
        {
            HandleInput(State.Guard);
        }

        public void Pierce()
        {
            HandleInput(State.Skill);
        }

        private void HandleInput(State input)
        {
            if (CurrentState == State.Idle)
            {
                if (input == State.Attack)
                {
                    CurrentState = State.Attack;
                    nowCoroutine = StartCoroutine(AttackAnimation());
                }
                else if (input == State.Guard)
                {
                    CurrentState = State.Guard;
                    nowCoroutine = StartCoroutine(GuardAnimation());
                }
                else if (input == State.Skill)
                {

                }
            }
            else if (CurrentState == State.Guard)
            {
                if (input == State.Attack)
                {

                }
            }
        }


        // private void SelectAttack()
        // {
        //     //적이 범위에 있을 경우
        //     if (Battle.IsEnemyInRangeOld())
        //     {
        //         GameManager.isStart = true;
        //         //올바른 입력을 했을 경우
        //         if (Battle.getEnemyAction() == getPlayerState())
        //         {
        //             if (getPlayerState() == 1)
        //             {
        //                 ParticleManager.CreateHitParticle();
        //             }
        //             else if (getPlayerState() == 2)
        //             {
        //                 ParticleManager.CreateDefenseParticle();
        //                 SoundManager.PlayerSound("defense"); //막았을 때만 소리가 나기 위해 여기 존재
        //             }
        //             else if (getPlayerState() == 3)
        //             {
        //                 ParticleManager.CreateRedEnemyHitParticle();
        //             }
        //             Battle.EnemyDamaged(1);
        //             GameManager.TimeUp();
        //         }
        //         //잘못된 입력을 했을 경우
        //         else
        //         {
        //             GameManager.setTime(0);
        //             return;
        //         }
        //     }

        //     //공격 이펙트 및 사운드 출력
        //     switch (getPlayerState())
        //     {
        //         case 1:
        //             ParticleManager.CreateSlashParticle();
        //             SoundManager.PlayerSound("slash");
        //             break;
        //         case 3:
        //             ParticleManager.CreatePierceParticle();
        //             SoundManager.PlayerSound("pierce");
        //             break;
        //     }
        // }


        IEnumerator AttackAnimation()
        {
            while (!IsAnimation("Attack"))
                yield return null;

            while (GetNormalizedTime() < 0.3f)
                yield return null;

            if (enemyManager.IsEnemyInRange(BATTLE_RANGE))
            {
                GameManager.isStart = true;

                // 입력 성공
                if (enemyManager.BattleEnemy(State.Attack, out bool isEnemyDead))
                {
                    ParticleManager.CreateHitParticle();
                }
                // 입력 실패
                else
                {
                    // yield break;
                }
            }

            ParticleManager.CreateSlashParticle();
            SoundManager.PlayerSound("slash");

            while (GetNormalizedTime() < 0.8f)
                yield return null;

            CurrentState = State.Idle;
        }

        IEnumerator GuardAnimation()
        {
            while (!IsAnimation("Guard"))
                yield return null;

            while (GetNormalizedTime() < 0.3f)
                yield return null;

            if (enemyManager.IsEnemyInRange(BATTLE_RANGE))
            {
                GameManager.isStart = true;

                // 입력 성공
                if (enemyManager.BattleEnemy(State.Guard, out bool isEnemyDead))
                {
                    ParticleManager.CreateDefenseParticle();
                    SoundManager.PlayerSound("defense");
                }
                // 입력 실패
                else
                {
                    // yield break;
                }
            }

            while (GetNormalizedTime() < 0.8f)
                yield return null;

            CurrentState = State.Idle;
        }

        private bool IsAnimation(string animation) => animator.GetCurrentAnimatorStateInfo(0).IsName(animation);
        private float GetNormalizedTime() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
