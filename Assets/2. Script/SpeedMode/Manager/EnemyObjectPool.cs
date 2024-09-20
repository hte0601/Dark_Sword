using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class EnemyObjectPool : MonoBehaviour
    {
        private static EnemyObjectPool enemyObjectPool;
        private static Transform objectPoolTransform;

        [SerializeField] private SwordGoblin swordGoblinPrefab;
        [SerializeField] private FireGoblin fireGoblinPrefab;
        [SerializeField] private SpearGoblin spearGoblinPrefab;

        // [SerializeField] private GameObject arrow;

        private Dictionary<Enemy.Type, EnemyPool> enemyDict = new();


        private void Awake()
        {
            enemyObjectPool = this;
            objectPoolTransform = transform;

            enemyDict.Add(Enemy.Type.SwordGoblin, new EnemyPool(swordGoblinPrefab, 16));
            enemyDict.Add(Enemy.Type.FireGoblin, new EnemyPool(fireGoblinPrefab, 16));
            enemyDict.Add(Enemy.Type.SpearGoblin, new EnemyPool(spearGoblinPrefab, 16));
        }

        private void OnDestroy()
        {
            enemyObjectPool = null;
            objectPoolTransform = null;
        }


        public Enemy GetEnemy(Enemy.Type enemyType, bool isObjectActive = true)
        {
            return enemyDict[enemyType].GetEnemy(isObjectActive);
        }

        public void ReturnEnemy(Enemy enemy)
        {
            enemyDict[enemy.EnemyType].ReturnEnemy(enemy);
        }


        private class EnemyPool
        {
            private readonly Enemy enemyPrefab;
            private readonly Queue<Enemy> enemyQueue = new();

            public EnemyPool(Enemy enemyPrefab, int preCreateNumber)
            {
                this.enemyPrefab = enemyPrefab;

                for (int i = 0; i < preCreateNumber; i++)
                {
                    enemyQueue.Enqueue(CreateEnemy());
                }
            }

            public Enemy GetEnemy(bool isObjectActive)
            {
                Enemy enemy;

                if (enemyQueue.Count > 0)
                    enemy = enemyQueue.Dequeue();
                else
                    enemy = CreateEnemy();

                enemy.transform.SetParent(null);
                enemy.gameObject.SetActive(isObjectActive);

                return enemy;
            }

            public void ReturnEnemy(Enemy enemy)
            {
                enemy.gameObject.SetActive(false);
                enemy.transform.SetParent(objectPoolTransform);

                enemyQueue.Enqueue(enemy);
            }

            private Enemy CreateEnemy()
            {
                Enemy enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, objectPoolTransform);
                enemy.gameObject.SetActive(false);
                enemy.objectPool = enemyObjectPool;

                return enemy;
            }
        }
    }
}
