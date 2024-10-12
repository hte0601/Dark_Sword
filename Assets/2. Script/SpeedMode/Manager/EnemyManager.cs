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
        private Wave currentWaveData;
        private Enemy.Types currentEliteEnemy;
        private Enemy.Types previousEnemy;

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

            currentEliteEnemy = Enemy.Types.SpearGoblin;
        }

        private void Start()
        {
            GameManager.instance.ReadyWaveEvent += HandleReadyWaveEvent;
            GameManager.instance.StartWaveEvent += HandleStartWaveEvent;
            GameManager.instance.RestartGameEvent += HandleRestartGameEvent;
            Swordman.instance.BattleEnemyEvent += HandleBattleEnemyEvent;
        }


        private void HandleReadyWaveEvent(Wave waveData)
        {
            currentWaveData = waveData;
            createEnemyNumber = currentWaveData.enemyNumber;
            RemainingEnemyNumber = currentWaveData.enemyNumber;
            previousEnemy = Enemy.Types.None;
        }

        private void HandleStartWaveEvent(int wave)
        {
            for (int i = 0; i < GameData.EnemyData.MAX_ENEMY_NUMBER; i++)
                CreateEnemy();
        }

        private void HandleRestartGameEvent()
        {
            ClearEnemy();
        }

        private void HandleBattleEnemyEvent(BattleReport battleReport)
        {
            // 적이 죽었거나 도망간 경우
            if (battleReport.IsEnemyRemoved())
            {
                RemoveEnemy();
            }
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


        private void CreateEnemy()
        {
            if (createEnemyNumber == 0)
                return;

            createEnemyNumber -= 1;

            Enemy.Types enemyType = RandomEnemy();

            if (enemyType == Enemy.Types.EliteEnemy)
                enemyType = currentEliteEnemy;

            Enemy enemy = enemyObjectPool.GetEnemy(enemyType);
            enemy.transform.position = GameData.EnemyData.ENEMY_CREATE_POSITION;

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
            RemainingEnemyNumber -= 1;

            if (enemyList.Count > 0)
            {
                enemyList[0].isHead = true;
                enemyList[0].frontEnemyTransform = null;
            }
            // 웨이브의 마지막 적이었다면
            else if (createEnemyNumber == 0)
            {
                GameManager.instance.RaiseEndWaveEvent(currentWaveData.wave);
                return;
            }

            CreateEnemy();
        }

        private void ClearEnemy()
        {
            for (int i = 0; i < enemyList.Count; i++)
                enemyObjectPool.ReturnEnemy(enemyList[i]);

            enemyList.Clear();
            RemainingEnemyNumber = 0;
        }


        private Enemy.Types RandomEnemy()
        {
            if (Enemy.Types.CommonEnemy.HasFlag(previousEnemy) && previousEnemy != Enemy.Types.None)
            {
                // 연속성이 양수이고 확률이 발동한 경우
                if (currentWaveData.continuity > Tool.Random.Value())
                {
                    return previousEnemy;
                }
                // 연속성이 음수이고 확률이 발동한 경우
                else if (-currentWaveData.continuity > Tool.Random.Value())
                {
                    previousEnemy = ChooseRandomEnemy(previousEnemy);
                    return previousEnemy;
                }
            }

            previousEnemy = ChooseRandomEnemy();
            return previousEnemy;
        }

        private Enemy.Types ChooseRandomEnemy(Enemy.Types exceptEnemyType = Enemy.Types.None)
        {
            float max = currentWaveData.enemyRateSum;

            if (exceptEnemyType != Enemy.Types.None)
                max -= currentWaveData.enemyRateDict[exceptEnemyType];

            float randomValue = Tool.Random.Range(0f, max);

            foreach (var item in currentWaveData.enemyRateDict)
            {
                if (item.Key == exceptEnemyType)
                    continue;

                if (randomValue < item.Value)
                    return item.Key;

                randomValue -= item.Value;
            }

#if UNITY_EDITOR
            Debug.Log("RandomEnemyType() 확률 오류");
#endif

            return Enemy.Types.EliteEnemy;
        }
    }
}
