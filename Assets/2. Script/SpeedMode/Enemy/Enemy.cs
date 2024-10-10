using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class Enemy : MonoBehaviourExt
    {
        [Flags]
        public enum Types
        {
            None = 0,

            SwordGoblin = 1 << 0,
            FireGoblin = 1 << 1,
            CommonEnemy = SwordGoblin | FireGoblin,

            SpearGoblin = 1 << 2,
            Elite2 = 1 << 3,
            EliteEnemy = SpearGoblin | Elite2
        }

        [SerializeField] protected GameObject model;
        [SerializeField] private Animator animator;

        public EnemyObjectPool objectPool;
        public Transform frontEnemyTransform;

        private Types _enemyType;
        protected Dictionary<int, Swordman.State> correctInput;

        private float moveSpeed;
        protected int maxHealth;
        protected bool canEscape;

        private int _currentHealth;
        public bool isStopped;
        public bool isHead;


        public Types EnemyType
        {
            get => _enemyType;
            protected set => _enemyType = value;
        }

        public Swordman.State CorrectInput
        {
            get => correctInput[CurrentHealth];
        }

        public int CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                if (value < 0)
                    _currentHealth = 0;
                else
                    _currentHealth = value;

                animator.SetInteger("Health", _currentHealth);
            }
        }


        protected virtual void Awake()
        {
            moveSpeed = GameData.EnemyData.ENEMY_MOVE_SPEED;
        }

        protected virtual void OnEnable()
        {
            CurrentHealth = maxHealth;
            isStopped = false;
        }

        protected virtual void Update()
        {
            Move();
        }

        private void Move()
        {
            if (isStopped)
                return;

            Vector3 moveTargetPosition;

            if (isHead)
                moveTargetPosition = GameData.EnemyData.ENEMY_MOVE_TARGET_POSITION;
            else
                moveTargetPosition = frontEnemyTransform.position + GameData.EnemyData.ENEMY_ENEMY_GAP;

            if (transform.position.x > moveTargetPosition.x)
                transform.position = Vector3.MoveTowards(transform.position, moveTargetPosition, moveSpeed * Time.deltaTime);
        }


        // 플레이어가 올바른 입력을 했는지 여부 반환
        public bool Battle(Swordman.State playerInput, out BattleReport.EnemyState enemyState)
        {
            if (playerInput == CorrectInput)
            {
                CurrentHealth -= 1;

                if (CurrentHealth == 0)
                {
                    Die();
                    enemyState = BattleReport.EnemyState.Killed;
                }
                // 피해를 입고 체력이 남았을 경우
                else
                {
                    enemyState = BattleReport.EnemyState.Alive;
                }

                return true;
            }
            else
            {
                if (canEscape)
                {
                    Escape();
                    enemyState = BattleReport.EnemyState.Escaped;
                }
                else
                {
                    enemyState = BattleReport.EnemyState.Alive;
                }

                return false;
            }
        }

        public int HitBySkill()
        {
            int damage = CurrentHealth;

            CurrentHealth = 0;
            objectPool.ReturnEnemy(this);

            return damage;
        }

        protected virtual void Die()
        {
            objectPool.ReturnEnemy(this);
        }

        protected virtual void Escape() { }
    }
}
