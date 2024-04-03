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

        private int createEnemyNumber;

        private void Awake()
        {
            instance = this;

            currentEliteEnemy = Enemy.Type.SpearGoblin;
        }

        private void Start()
        {
            GameManager.instance.ReadyWaveEvent += HandleReadyWaveEvent;
            GameManager.instance.StartWaveEvent += HandleStartWaveEvent;
            GameManager.instance.RestartGameEvent += HandleRestartGameEvent;
            Swordman.instance.BattleEnemyEvent += HandleBattleEnemyEvent;
        }

        private void HandleReadyWaveEvent(int wave)
        {
            currentWave = ModeData.WaveData.waves[wave];
            createEnemyNumber = currentWave.ENEMY_NUMBER;
        }

        private void HandleStartWaveEvent(int wave)
        {
            for (int i = 0; i < ModeData.EnemyData.MAX_ENEMY_NUMBER; i++)
                CreateEnemy();
        }

        private void HandleRestartGameEvent()
        {
            ClearEnemy();
        }

        public bool IsEnemyInRange(float battleRange)
        {
            if (enemyList.Count > 0)
                if (enemyList[0].transform.position.x < battleRange)
                    return true;

            return false;
        }

        public Enemy GetHeadEnemy()
        {
            return enemyList[0];
        }

        private void HandleBattleEnemyEvent(BattleReport battleReport)
        {
            if (battleReport.isEnemyDead)
                RemoveEnemy();
        }

        private void CreateEnemy()
        {
            if (createEnemyNumber == 0)
                return;

            createEnemyNumber -= 1;

            Enemy.Type enemyType = currentWave.RandomEnemy();

            if (enemyType == Enemy.Type.EliteEnemy)
                enemyType = currentEliteEnemy;

            Enemy enemy = enemyObjectPool.GetEnemy(enemyType);
            enemy.transform.position = ModeData.EnemyData.ENEMY_CREATE_POSITION;

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

            if (enemyList.Count > 0)
            {
                enemyList[0].isHead = true;
                enemyList[0].frontEnemyTransform = null;
            }
            // 웨이브의 마지막 적이었다면
            else if (createEnemyNumber == 0)
            {
                GameManager.instance.RaiseEndWaveEvent(currentWave.WAVE);
                return;
            }

            CreateEnemy();
        }

        private void ClearEnemy()
        {
            for (int i = 0; i < enemyList.Count; i++)
                enemyObjectPool.ReturnEnemy(enemyList[i]);

            enemyList.Clear();
        }
    }
}
