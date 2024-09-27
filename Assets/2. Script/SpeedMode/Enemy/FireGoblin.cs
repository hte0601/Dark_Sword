
using UnityEngine;

namespace SpeedMode
{
    public class FireGoblin : Enemy
    {
        [SerializeField] private ParticleSystem explosionParticle;

        protected override void Awake()
        {
            base.Awake();

            EnemyType = Types.FireGoblin;
            maxHealth = 1;
            canEscape = true;

            correctInput = new()
            {
                {1, Swordman.State.Guard}
            };
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            model.SetActive(true);
        }

        protected override void Die()
        {
            Explode();
        }

        protected override void Escape()
        {
            Explode();
        }

        private void Explode()
        {
            model.SetActive(false);
            explosionParticle.Play();

            DelayInvoke(1f, objectPool.ReturnEnemy, this);
        }
    }
}
