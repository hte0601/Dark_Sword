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

        public EnemyObjectPool objectPool;
        [SerializeField] protected GameObject model;
        [SerializeField] private Animator animator;

        private Types _enemyType;
        protected Dictionary<int, Swordman.State> correctInput;

        private float moveSpeed;
        protected int MAX_HEALTH;
        private int _currentHealth;

        public bool isStopped;
        public bool isHead;
        public Transform frontEnemyTransform;

        public Swordman.State CorrectInput
        {
            get => correctInput[CurrentHealth];
        }

        public Types EnemyType
        {
            get => _enemyType;
            protected set => _enemyType = value;
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
            moveSpeed = ModeData.EnemyData.ENEMY_MOVE_SPEED;
        }

        protected virtual void OnEnable()
        {
            CurrentHealth = MAX_HEALTH;
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
                moveTargetPosition = ModeData.EnemyData.ENEMY_MOVE_TARGET_POSITION;
            else
                moveTargetPosition = frontEnemyTransform.position + ModeData.EnemyData.ENEMY_ENEMY_GAP;

            if (transform.position.x > moveTargetPosition.x)
                transform.position = Vector3.MoveTowards(transform.position, moveTargetPosition, moveSpeed * Time.deltaTime);
        }


        public bool Battle(Swordman.State playerInput, out bool isEnemyDead)
        {
            if (playerInput == CorrectInput)
            {
                CurrentHealth -= 1;

                if (CurrentHealth == 0)
                {
                    Die();
                    isEnemyDead = true;
                }
                else
                {
                    // 피해를 입고 체력이 남았을 경우 처리가 필요하다면 여기에
                    isEnemyDead = false;
                }

                return true;
            }
            else
            {
                RunAway();
                isEnemyDead = false;
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

        protected virtual void RunAway() { }
    }
}
