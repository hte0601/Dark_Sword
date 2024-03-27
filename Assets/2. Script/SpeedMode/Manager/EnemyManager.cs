using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;
        [SerializeField] private EnemyObjectPool enemyObjectPool;

        private List<Enemy> enemyList = new();
        private Wave currentWave;
        private Enemy.Type currentEliteEnemy;

        private const float MAX_GAP = 1f;

        public event Action<Enemy.Type, bool, bool> FightEnemyEvent;

        private void Awake()
        {
            instance = this;
            currentWave = ModeData.WaveData.waves[16];
            currentEliteEnemy = Enemy.Type.SpearGoblin;
        }

        public bool IsEnemyInRange()
        {
            if (enemyList.Count > 0)
                if (enemyList[0].transform.position.x < GameManager.battlePos.x + MAX_GAP)
                    return true;

            return false;
        }

        public bool FightEnemy(Swordman.State input, out bool isEnemyDead)
        {
            Enemy.Type enemyType = enemyList[0].enemyType;
            bool isInputCorrect;

            if (input == enemyList[0].CorrectInput)
            {
                isInputCorrect = true;
                isEnemyDead = enemyList[0].Damage(1);

                if (isEnemyDead)
                {
                    RemoveEnemy();
                    CreateEnemy();
                }
            }
            else
            {
                isInputCorrect = false;
                isEnemyDead = false;
            }

            FightEnemyEvent?.Invoke(enemyType, isInputCorrect, isEnemyDead);
            return isInputCorrect;
        }

        public void CreateEnemy()
        {
            Enemy.Type enemyType = currentWave.RandomEnemy();

            if (enemyType == Enemy.Type.EliteEnemy)
                enemyType = currentEliteEnemy;

            Enemy enemy = enemyObjectPool.GetEnemy(enemyType);
            enemy.transform.position = GameManager.createPos;

            if (enemyList.Count == 0)
            {
                enemy.isHead = true;
                enemy.frontEnemyTransform = null;
            }
            else
            {
                enemy.isHead = false;
                enemy.frontEnemyTransform = enemyList[^1].transform;
            }

            enemyList.Add(enemy);
        }

        private void RemoveEnemy()
        {
            enemyObjectPool.ReturnEnemy(enemyList[0]);
            enemyList.RemoveAt(0);

            enemyList[0].isHead = true;
            enemyList[0].frontEnemyTransform = null;
        }


        // 수정 필요
        public static void ClearEnemy()
        {
            // for (int i = 0; i < EnemyListOld.Count; i++)
            //     ObjectPool.ReturnObjectOld(EnemyListOld[i]);
            // EnemyListOld.Clear();
        }
    }
}
