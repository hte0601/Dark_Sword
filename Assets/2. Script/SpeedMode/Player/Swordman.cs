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
            SwordmanGroggy,
            GameOver,
            SkillCast,
            SkillAutoCast,
            SkillHit
        }

        public Enemy.Type? enemyType;
        public Swordman.State? playerInput;
        public Result result;
        public int damageDealt;
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
        private SwordmanEffect effect;

        private Coroutine nowAnimationCoroutine;
        private float battleRange;

        private int currentHealth;
        private int _attackCombo = 0;
        private bool canPierceCombo = false;
        private int skillGauge;
        private int skillAutoCastNumber;  // 임시

        [SerializeField] private SlashEffect slashEffect;  // 임시

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
            effect = transform.Find("Effect").GetComponent<SwordmanEffect>();

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


        // 버튼 입력 (모바일)
        public void AttackButtonInput()
        {
            HandleInput(State.Attack);
        }

        public void GuardButtonInput()
        {
            HandleInput(State.Guard);
        }

        public void SkillButtonInput()
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
                    CurrentState = State.Skill;
                    animator.CrossFadeInFixedTime("Skill", 0.05f, 0);
                    nowAnimationCoroutine = StartCoroutine(SkillAnimation());
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
                enemyType = enemy.EnemyType,
                playerInput = playerInput,
                damageDealt = 0,
                isEnemyDead = false
            };

            // 입력 성공
            if (playerInput == enemy.CorrectInput)
            {
                battleReport.result = BattleReport.Result.InputCorrect;
                battleReport.damageDealt = 1;
                battleReport.isEnemyDead = enemy.TakeDamage(1);
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

            if (currentHealth > 0)
            {
                CurrentState = State.Groggy;
                nowAnimationCoroutine = StartCoroutine(GroggyAnimation());
                return BattleReport.Result.SwordmanGroggy;
            }
            else
            {
                currentHealth = 0;
                CurrentState = State.Die;
                return BattleReport.Result.GameOver;
            }
        }

        private IEnumerator CastSkill()
        {
            List<Enemy> enemies = enemyManager.GetEnemies(8);
            
            for (int i = 0; i < enemies.Count; i++)
            {
                StartCoroutine(SkillHitEnemy(enemies[i]));
                yield return new WaitForSeconds(0.06f);
            }
        }

        private IEnumerator SkillHitEnemy(Enemy enemy)
        {
            BattleReport skillHitReport = new()
            {
                enemyType = enemy.EnemyType,
                playerInput = null,
                result = BattleReport.Result.SkillHit,
                isEnemyDead = true
            };

            enemy.isStopped = true;
            SlashEffect effect = Instantiate(slashEffect, enemy.transform.position + new Vector3(0, 0, -1), Quaternion.Euler(0f, 0f, -60f));
            effect.Play(SlashEffect.Color.Purple);
            SoundManager.PlayerSound("slash");

            yield return new WaitForSeconds(0.06f);

            skillHitReport.damageDealt = enemy.HitBySkill();
            BattleEnemyEvent?.Invoke(skillHitReport);
        }


        private IEnumerator AttackAnimation()
        {
            const int animationLength = 10;

            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (WaitUntilFrame(2, animationLength))
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

            effect.PlaySlashEffect();
            SoundManager.PlayerSound("slash");

            while (NormalizedTime() < 0.475f)
                yield return null;

            CurrentState = State.Idle;
        }

        private IEnumerator PierceAnimation()
        {
            const int animationLength = 10;

            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (WaitUntilFrame(2, animationLength))
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

            effect.PlayPierceEffect();
            SoundManager.PlayerSound("slash");

            while (NormalizedTime() < 0.475f)
                yield return null;

            CurrentState = State.Idle;
        }

        private IEnumerator GuardAnimation()
        {
            const int animationLength = 10;

            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (WaitUntilFrame(2, animationLength))
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

            while (NormalizedTime() < 0.475f)
                yield return null;

            canPierceCombo = false;
            CurrentState = State.Idle;
        }

        private IEnumerator SkillAnimation()
        {
            const int animationLength = 60;

            BattleReport SkillCastReport = new()
            {
                enemyType = null,
                playerInput = State.Skill,
                result = BattleReport.Result.SkillCast,
                damageDealt = 0,
                isEnemyDead = false
            };

            BattleEnemyEvent?.Invoke(SkillCastReport);

            yield return null;

            while (animator.IsInTransition(0))
                yield return null;

            while (WaitUntilFrame(33, animationLength))
                yield return null;

            StartCoroutine(CastSkill());

            while (IsAnimation("Skill") && !animator.IsInTransition(0))
                yield return null;

            CurrentState = State.Idle;
        }

        private IEnumerator GroggyAnimation()
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
        private float NormalizedTime(int layerIndex = 0) => animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
        private float CurrentAnimationFrame(int animationLength, int layerIndex = 0) => animationLength * NormalizedTime(layerIndex);
        private bool WaitUntilFrame(int frame, int animationLength, int layerIndex = 0) => animationLength * NormalizedTime(layerIndex) < frame - 0.25f;
    }
}
