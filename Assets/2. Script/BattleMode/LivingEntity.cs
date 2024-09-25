using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMode
{
    public class LivingEntity : MonoBehaviour
    {
        protected Animator animator;

        protected float maxHealth;
        protected float health;
        protected float speed;
        protected float damage;
        protected float stamina;

        protected bool isAlreadyDamage;
        protected bool isAlreadyPush;
        protected bool isGroggyAttack;
        protected bool isDead;

        protected void SetStatus(float health, float speed, float damage, float stamina = 100f)
        {
            maxHealth = health;
            this.health = health;
            this.speed = speed;
            this.damage = damage;
            this.stamina = stamina;
            isAlreadyDamage = true;
            isAlreadyPush = true;
            isGroggyAttack = false;
            isDead = false;
        }

        protected void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        protected virtual void Die() { }

        public virtual void OnDamage(float dmg)
        {
            health -= dmg;
            if (health <= 0 && !isDead)
                Die();
        }

        protected int getState()
        {
            return animator.GetInteger("State");
        }

        protected void setState(int state)
        {
            animator.SetInteger("State", state);
        }

        protected bool isAnimation(string anima)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(anima);
        }

        protected float getNormalizedTime()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}
