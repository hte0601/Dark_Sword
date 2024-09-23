using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class Wave
    {
        public int wave;  // 무한 모드를 위해 임시로 readonly 제거
        public readonly float timerSpeed;

        public readonly int enemyNumber;
        public readonly Dictionary<Enemy.Types, float> enemyRateDict;
        public readonly float enemyRateSum;

        public readonly float continuity;


        public Wave(
            int wave, float timerSpeed, int enemyNumber,
            float swordGoblinRate, float fireGoblinRate, float eliteGoblinRate,
            float continuity = 0f)
        {
            this.wave = wave;
            this.timerSpeed = timerSpeed;

            this.enemyNumber = enemyNumber;

            enemyRateDict = new()
            {
                {Enemy.Types.SwordGoblin, swordGoblinRate},
                {Enemy.Types.FireGoblin, fireGoblinRate},
                {Enemy.Types.EliteEnemy, eliteGoblinRate}
            };

            enemyRateSum = swordGoblinRate + fireGoblinRate + eliteGoblinRate;

            this.continuity = continuity;
        }
    }
}
