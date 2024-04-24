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

            if (effectColor == Color.Red)
                animator.Play("Red_Slash");
            else if (effectColor == Color.Green)
                animator.Play("Green_Slash");
            else if (effectColor == Color.Blue)
                animator.Play("Blue_Slash");
            else if (effectColor == Color.Purple)
                animator.Play("Purple_Slash");

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
