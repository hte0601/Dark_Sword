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

        [SerializeField] private Animator animator;

        public Type enemyType;
        protected Dictionary<int, Swordman.State> correctInput = new();

        protected float moveSpeed;
        protected int MAX_HEALTH;
        protected int currentHealth;
        public bool isHead = false;
        public Transform frontEnemyTransform;

        public Swordman.State CorrectInput
        {
            get => correctInput[CurrentHealth];
        }

        public int CurrentHealth
        {
            get => currentHealth;
            private set
            {
                currentHealth = value;
                animator.SetInteger("Health", currentHealth);
            }
        }


        protected virtual void Awake()
        {
            moveSpeed = ModeData.EnemyData.ENEMY_MOVE_SPEED;
        }

        protected virtual void OnEnable()
        {
            CurrentHealth = MAX_HEALTH;
        }

        protected virtual void Update()
        {
            //자기 자신이 가장 앞에 있는 enemy일 경우 battlePos로 이동
            if (isHead)
                transform.position = Vector3.MoveTowards(transform.position, ModeData.EnemyData.ENEMY_MOVE_TARGET_POSITION, moveSpeed * Time.deltaTime);
            //그렇지 않을 경우 자기 앞에 있는 enemy의 위치에 x좌표 +2한 위치로 이동
            else
                transform.position = Vector3.MoveTowards(transform.position, frontEnemyTransform.position + ModeData.EnemyData.ENEMY_ENEMY_GAP, moveSpeed * Time.deltaTime);
        }

        public bool TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                Die();
                return true;
            }

            return false;
        }

        private void Die()
        {
            CurrentHealth = 0;
            SoundManager.EnemySound();
        }
    }
}
