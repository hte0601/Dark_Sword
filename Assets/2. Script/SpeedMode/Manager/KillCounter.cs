using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpeedMode
{
    public class KillCounter : MonoBehaviour
    {
        public static KillCounter instance;

        public event Action<int> OnKillCountValueChanged;

        private readonly Dictionary<Enemy.Type, int> killCountDict = new();
        private int _killCount = 0;

        public int KillCount
        {
            get => _killCount;
            private set
            {
                _killCount = value;
                OnKillCountValueChanged?.Invoke(_killCount);
            }
        }


        private void Awake()
        {
            instance = this;

            foreach (Enemy.Type enemyType in Enum.GetValues(typeof(Enemy.Type)))
            {
                if (enemyType != Enemy.Type.None)
                    killCountDict.Add(enemyType, 0);
            }
        }

        private void Start()
        {
            GameManager.instance.GameOverEvent += HandleGameOverEvent;
            GameManager.instance.RestartGameEvent += HandleRestartGameEvent;
            Swordman.instance.BattleEnemyEvent += CountKill;
        }


        private void HandleGameOverEvent()
        {
            // 통계 데이터 수정 및 저장 구현 필요
        }

        private void HandleRestartGameEvent()
        {
            // foreach (KeyValuePair<Enemy.Type, int> item in killCountDict)
            //     Debug.Log(string.Format("{0}, {1}", item.Key, item.Value));

            ResetKillCount();
        }


        private void CountKill(BattleReport battleReport)
        {
            if (!battleReport.isEnemyDead)
                return;

            KillCount += 1;
            killCountDict[battleReport.enemyType] += 1;

            if (Enemy.Type.CommonEnemy.HasFlag(battleReport.enemyType))
                killCountDict[Enemy.Type.CommonEnemy] += 1;

            if (Enemy.Type.EliteEnemy.HasFlag(battleReport.enemyType))
                killCountDict[Enemy.Type.EliteEnemy] += 1;
        }

        private void ResetKillCount()
        {
            foreach (var key in killCountDict.Keys.ToList())
            {
                killCountDict[key] = 0;
            }
        }

        public int GetKillCount(Enemy.Type enemyType)
        {
            return killCountDict[enemyType];
        }
    }
}