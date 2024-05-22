using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;

        public event Action<int> OnRemainingEnemyNumberChanged;

        [SerializeField] private EnemyObjectPool enemyObjectPool;

        private readonly List<Enemy> enemyList = new();
        private Wave currentWave;
        private Enemy.Type currentEliteEnemy;

        private int createEnemyNumber;
        private int _remainingEnemyNumber = 0;

        public int RemainingEnemyNumber
        {
            get => _remainingEnemyNumber;
            private set
            {
                _remainingEnemyNumber = value;
                OnRemainingEnemyNumberChanged?.Invoke(_remainingEnemyNumber);
            }
        }


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

        private void HandleReadyWaveEvent(Wave wave)
        {
            currentWave = wave;
            createEnemyNumber = currentWave.ENEMY_NUMBER;
            RemainingEnemyNumber = currentWave.ENEMY_NUMBER;
        }

        private void HandleStartWaveEvent(int wave)
        {
            for (int i = 0; i < ModeData.EnemyData.MAX_ENEMY_NUMBER; i++)
                CreateEnemy();
        }

        private void HandleRestartGameEvent()
        {
            ClearEnemy();
            RemainingEnemyNumber = 0;
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
            if (enemyList.Count > 0)
                return enemyList[0];
            else
                return null;
        }

        public List<Enemy> GetEnemies(int number)
        {
            List<Enemy> enemies = new();

            for (int i = 0; i < Math.Min(number, enemyList.Count); i++)
                enemies.Add(enemyList[i]);

            return enemies;
        }

        private void HandleBattleEnemyEvent(BattleReport battleReport)
        {
            // 적이 죽었거나 입력 실패인 경우
            if (battleReport.isEnemyDead || battleReport.IsInputIncorrect())
            {
                RemoveEnemy();
                RemainingEnemyNumber -= 1;
            }
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
