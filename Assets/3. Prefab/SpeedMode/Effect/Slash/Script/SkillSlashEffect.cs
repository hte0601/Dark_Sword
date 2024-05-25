using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SkillSlashEffect : SlashEffect, IPoolableObject
    {
        private ObjectPool<SkillSlashEffect> pool;

        public void SetPool(object pool) => this.pool = (ObjectPool<SkillSlashEffect>)pool;

        public void Play(Vector3 position, Color effectColor = Color.Red)
        {
            transform.position = position;

            PlaySlashAnimation(effectColor);
            PlaySoundEffect();

            StartCoroutine(ReturnObject());
        }

        public IEnumerator ReturnObject()
        {
            yield return null;

            while (!IsAnimation("Idle"))
                yield return null;

            pool.ReturnObject(this);
        }

        private bool IsAnimation(string animation, int layerIndex = 0) => animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animation);
    }
}
