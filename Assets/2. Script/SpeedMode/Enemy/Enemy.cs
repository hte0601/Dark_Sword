using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class Enemy : MonoBehaviour
    {
        public enum Type
        {
            SwordGoblin,
            FireGoblin,
            EliteEnemy,
            SpearGoblin
        }

        public EnemyObjectPool objectPool;
        [SerializeField] private Animator animator;

        private Type _enemyType;
        protected Dictionary<int, Swordman.State> correctInput = new();

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

        public Type EnemyType
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

        public bool TakeDamage(int damage = 1)
        {
            CurrentHealth -= damage;

            if (CurrentHealth == 0)
            {
                Die();
                return true;
            }

            return false;
        }

        public int HitBySkill()
        {
            int damage = CurrentHealth;

            CurrentHealth = 0;
            objectPool.ReturnEnemy(this);

            return damage;
        }

        private void Die()
        {
            SoundManager.EnemySound();
            objectPool.ReturnEnemy(this);
        }
    }
}
