using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public struct BattleReport
    {
        public enum Result
        {
            InputCorrect,
            SkillAutoCast,
            SwordmanGroggy,
            GameOver
        }

        public Enemy.Type enemyType;
        public Swordman.State playerInput;
        public Result result;
        public bool isEnemyDead;
    }

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

        public event Action<BattleReport> BattleEnemyEvent;

        private EnemyManager enemyManager;
        private UpgradeData upgrades;
        private Animator animator;

        private Coroutine nowAnimationCoroutine;
        private float battleRange;

        private int currentHealth;
        private int _attackCombo = 0;
        private bool canPierceCombo = false;
        private int skillGauge;
        private int skillAutoCastNumber;  // 임시

        public State CurrentState
        {
            get => (State)animator.GetInteger("State");
            private set => animator.SetInteger("State", (int)value);
        }

        public int AttackCombo
        {
            get => _attackCombo;
            private set => _attackCombo = value % 2;
        }


        private void Awake()
        {
            instance = this;
            animator = transform.Find("model").GetComponent<Animator>();

            battleRange = transform.position.x + ModeData.SwordmanData.MAX_BATTLE_RANGE;
        }

        private void Start()
        {
            enemyManager = EnemyManager.instance;
            upgrades = SaveData.instance.upgrades;

            GameManager.instance.RestartGameEvent += HandleRestartGameEvent;

            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.J))
                HandleInput(State.Attack);

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.K))
                HandleInput(State.Guard);

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.L))
                HandleInput(State.Skill);
        }

        private void Initialize()
        {
            currentHealth = upgrades.maxHealth;
            skillAutoCastNumber = upgrades.skillAutoCastNumber;

            AttackCombo = 0;
            canPierceCombo = false;

            CurrentState = State.Idle;
        }

        private void HandleRestartGameEvent()
        {
            Initialize();
        }

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

        // 테스트를 위해 일단 public으로
        public void HandleInput(State input)
        {
            if (CurrentState == State.Idle)
            {
                if (input == State.Attack)
                {
                    CurrentState = State.Attack;

                    if (AttackCombo == 0)
                    {
                        animator.CrossFadeInFixedTime("Attack", 0.034f, 0);
                        nowAnimationCoroutine = StartCoroutine(AttackAnimation());
                    }
                    else
                    {
                        animator.CrossFadeInFixedTime("Pierce", 0.034f, 0);
                        nowAnimationCoroutine = StartCoroutine(PierceAnimation());
                    }

                    AttackCombo += 1;
                }
                else if (input == State.Guard)
                {
                    CurrentState = State.Guard;
                    animator.CrossFadeInFixedTime("Guard", 0.034f, 0);
                    nowAnimationCoroutine = StartCoroutine(GuardAnimation());
                    AttackCombo = 0;
                }
                else if (input == State.Skill)
                {
                    AttackCombo = 0;
                }
            }
            else if (canPierceCombo && input == State.Attack)
            {
                CurrentState = State.Attack;
                canPierceCombo = false;
                animator.CrossFadeInFixedTime("Pierce", 0.034f, 0);

                StopAnimationCoroutine();
                nowAnimationCoroutine = StartCoroutine(PierceAnimation());
                AttackCombo = 0;
            }
        }

        private BattleReport BattleEnemy(State playerInput)
        {
            Enemy enemy = enemyManager.GetHeadEnemy();

            BattleReport battleReport = new()
            {
                enemyType = enemy.enemyType,
                playerInput = playerInput,
                isEnemyDead = false
            };

            // 입력 성공
            if (playerInput == enemy.CorrectInput)
            {
                battleReport.result = BattleReport.Result.InputCorrect;
                battleReport.isEnemyDead = enemy.TakeDamage();
            }
            else
            {
                battleReport.result = TakeDamage();
            }

            BattleEnemyEvent?.Invoke(battleReport);
            return battleReport;
        }

        public BattleReport.Result TakeDamage(int damage = 1)
        {
            StopAnimationCoroutine();

            if (skillAutoCastNumber > 0)
            {
                // 스킬 자동 시전 처리
                return BattleReport.Result.SkillAutoCast;
            }

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                CurrentState = State.Die;
                return BattleReport.Result.GameOver;
            }
            else
            {
                CurrentState = State.Groggy;
                nowAnimationCoroutine = StartCoroutine(GroggyAnimation());
                return BattleReport.Result.SwordmanGroggy;
            }
        }


        IEnumerator AttackAnimation()
        {
            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            // Debug.Log(string.Format("트랜지션 종료 {0}", GetNormalizedTime()));

            while (GetNormalizedTime() < 0.175f)
                yield return null;

            if (enemyManager.IsEnemyInRange(battleRange))
            {
                BattleReport battleReport = BattleEnemy(State.Attack);

                // 입력 성공
                if (battleReport.result == BattleReport.Result.InputCorrect)
                {
                    ParticleManager.CreateHitParticle();
                }
                // 입력 실패
                else
                {
                    yield break;
                }
            }

            // Debug.Log(string.Format("배틀 처리 완료 {0}", GetNormalizedTime()));

            ParticleManager.CreateSlashParticle();
            SoundManager.PlayerSound("slash");

            while (GetNormalizedTime() < 0.475f)
                yield return null;

            CurrentState = State.Idle;
        }

        IEnumerator PierceAnimation()
        {
            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (GetNormalizedTime() < 0.175f)
                yield return null;

            if (enemyManager.IsEnemyInRange(battleRange))
            {
                BattleReport battleReport = BattleEnemy(State.Attack);

                // 입력 성공
                if (battleReport.result == BattleReport.Result.InputCorrect)
                {
                    // 메소드명 변경 필요
                    ParticleManager.CreateRedEnemyHitParticle();
                }
                // 입력 실패
                else
                {
                    yield break;
                }
            }

            ParticleManager.CreatePierceParticle();
            SoundManager.PlayerSound("slash");

            while (GetNormalizedTime() < 0.475f)
                yield return null;

            CurrentState = State.Idle;
        }

        IEnumerator GuardAnimation()
        {
            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (GetNormalizedTime() < 0.175f)
                yield return null;

            if (enemyManager.IsEnemyInRange(battleRange))
            {
                BattleReport battleReport = BattleEnemy(State.Guard);

                // 입력 성공
                if (battleReport.result == BattleReport.Result.InputCorrect)
                {
                    ParticleManager.CreateDefenseParticle();
                    SoundManager.PlayerSound("defense");

                    if (battleReport.enemyType == Enemy.Type.SpearGoblin)
                        if (!battleReport.isEnemyDead)
                        {
                            canPierceCombo = true;
                            AttackCombo = 1;
                        }
                }
                // 입력 실패
                else
                {
                    yield break;
                }
            }

            while (GetNormalizedTime() < 0.475f)
                yield return null;

            canPierceCombo = false;
            CurrentState = State.Idle;
        }

        IEnumerator GroggyAnimation()
        {
            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (IsAnimation("Groggy") && !animator.IsInTransition(0))
                yield return null;
            
            CurrentState = State.Idle;
        }


        private void StopAnimationCoroutine()
        {
            if (nowAnimationCoroutine != null)
                StopCoroutine(nowAnimationCoroutine);
        }

        private bool IsAnimation(string animation, int layerIndex = 0) => animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animation);
        private float GetNormalizedTime(int layerIndex = 0) => animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
    }
}
