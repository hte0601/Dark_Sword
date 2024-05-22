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
            SkillHit

            // SkillAutoCast,
        }

        public Enemy.Type enemyType;
        public Swordman.State? playerInput;
        public Result result;
        public int damageDealt;
        public bool isEnemyDead;

        public bool IsInputIncorrect()
        {
            return result == Result.SwordmanGroggy || result == Result.GameOver;
        }
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
        public event Action<int> OnCurrentHealthChanged;

        private EnemyManager enemyManager;
        private UpgradeData upgrades;
        private SwordmanAnimationController animationController;
        private SwordmanEffectController effectController;

        private float battleRange;

        private int _currentHealth = 0;
        private int skillGauge;


        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                if (value < 0)
                    _currentHealth = 0;
                else
                    _currentHealth = value;

                OnCurrentHealthChanged?.Invoke(_currentHealth);
            }
        }


        private void Awake()
        {
            instance = this;
            animationController = transform.Find("model").GetComponent<SwordmanAnimationController>();
            effectController = transform.Find("Effect").GetComponent<SwordmanEffectController>();

            upgrades = SaveDataManager.LoadData<UpgradeData>();

            battleRange = transform.position.x + ModeData.SwordmanData.MAX_BATTLE_RANGE;
        }

        private void Start()
        {
            enemyManager = EnemyManager.instance;

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
            CurrentHealth = upgrades.maxHealth;

            animationController.Initialize();
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
            if (animationController.CurrentState == State.Idle)
            {
                if (input == State.Attack || input == State.Guard)
                {
                    bool isEnemyInRange = enemyManager.IsEnemyInRange(battleRange);

                    if (isEnemyInRange)
                    {
                        // 입력에 실패했다면 애니메이션 재생x
                        if (BattleEnemy(input).result != BattleReport.Result.InputCorrect)
                            return;
                    }

                    if (input == State.Attack)
                    {
                        animationController.RunAttackAnimation(isEnemyInRange);
                    }
                    else if (input == State.Guard)
                    {
                        animationController.RunGuardAnimation(isEnemyInRange);
                    }
                }
                else if (input == State.Skill)
                {
                    // 스킬이 준비가 안 됐다면 return

                    BattleReport SkillCastReport = new()
                    {
                        enemyType = Enemy.Type.None,
                        playerInput = State.Skill,
                        result = BattleReport.Result.SkillCast,
                        damageDealt = 0,
                        isEnemyDead = false
                    };

                    BattleEnemyEvent?.Invoke(SkillCastReport);

                    animationController.RunSkillAnimation(CastSkill);
                }
            }
            else if (animationController.CanComboInput)
            {
                if (animationController.canSpearGoblinCombo && input == State.Attack)
                {
                    BattleEnemy(State.Attack);
                    animationController.RunSpearGoblinCombo();
                }
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
            };

            // 입력 성공
            if (enemy.Battle(playerInput, out battleReport.isEnemyDead))
            {
                battleReport.result = BattleReport.Result.InputCorrect;
                battleReport.damageDealt = 1;

                if (battleReport.enemyType == Enemy.Type.SpearGoblin)
                    if (battleReport.playerInput == State.Guard && !battleReport.isEnemyDead)
                        animationController.canSpearGoblinCombo = true;
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
            animationController.StopAnimation();

            // if (skillAutoCastNumber > 0)
            // {
            //     // 스킬 자동 시전 처리
            //     return BattleReport.Result.SkillAutoCast;
            // }

            CurrentHealth -= damage;

            if (CurrentHealth > 0)
            {
                animationController.RunGroggyAnimation();
                effectController.PlayHealthLossEffect();
                return BattleReport.Result.SwordmanGroggy;
            }
            else
            {
                animationController.RunDieAnimation();
                effectController.PlayHealthLossEffect();
                return BattleReport.Result.GameOver;
            }
        }


        private void CastSkill()
        {
            StartCoroutine(SkillCoroutine());
        }

        private IEnumerator SkillCoroutine()
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

            effectController.PlaySkillEffect(enemy.transform.position);

            yield return new WaitForSeconds(0.1f);

            skillHitReport.damageDealt = enemy.HitBySkill();
            BattleEnemyEvent?.Invoke(skillHitReport);
        }
    }
}
