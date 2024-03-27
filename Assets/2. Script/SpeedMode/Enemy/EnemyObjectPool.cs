using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class EnemyObjectPool : MonoBehaviour
    {
        private static Transform objectPoolTransform;

        [SerializeField] private SwordGoblin swordGoblin;
        [SerializeField] private FireGoblin fireGoblin;
        [SerializeField] private SpearGoblin spearGoblin;

        [SerializeField] private GameObject arrow;

        private Dictionary<Enemy.Type, EnemyPool> enemyDict = new();

        private void Awake()
        {
            objectPoolTransform = transform;

            enemyDict.Add(Enemy.Type.SwordGoblin, new EnemyPool(Enemy.Type.SwordGoblin, swordGoblin, 16));
            enemyDict.Add(Enemy.Type.FireGoblin, new EnemyPool(Enemy.Type.FireGoblin, fireGoblin, 16));
            enemyDict.Add(Enemy.Type.SpearGoblin, new EnemyPool(Enemy.Type.SpearGoblin, spearGoblin, 16));
        }

        public Enemy GetEnemy(Enemy.Type enemyType, bool isObjectActive = true)
        {
            return enemyDict[enemyType].GetEnemy(isObjectActive);
        }

        public void ReturnEnemy(Enemy enemy)
        {
            enemyDict[enemy.enemyType].ReturnEnemy(enemy);
        }


        private class EnemyPool
        {
            private readonly Enemy.Type enemyType;
            private readonly Enemy enemyPrefab;
            private readonly Queue<Enemy> enemyQueue = new();
            private readonly int PreCreateNumber;

            public EnemyPool(Enemy.Type enemyType, Enemy enemyPrefab, int preCreateNumber)
            {
                this.enemyType = enemyType;
                this.enemyPrefab = enemyPrefab;
                this.PreCreateNumber = preCreateNumber;

                for (int i = 0; i < PreCreateNumber; i++)
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
                enemy.enemyType = enemyType;

                return enemy;
            }
        }
    }
}