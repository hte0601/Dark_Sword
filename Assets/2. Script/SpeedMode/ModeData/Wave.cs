using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class Wave
    {
        public readonly int wave;
        public readonly float timerSpeed;

        public readonly int enemyNumber;
        public readonly Dictionary<Enemy.Type, float> enemyRateDict;
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
                {Enemy.Type.SwordGoblin, swordGoblinRate},
                {Enemy.Type.FireGoblin, fireGoblinRate},
                {Enemy.Type.EliteEnemy, eliteGoblinRate}
            };

            enemyRateSum = swordGoblinRate + fireGoblinRate + eliteGoblinRate;

            this.continuity = continuity;
        }
    }
}
