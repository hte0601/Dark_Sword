using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class Wave
    {
        public readonly int WAVE;
        public readonly float TIMER_SPEED;

        public readonly int ENEMY_NUMBER;
        private readonly float SWORD_GOBLIN_RATE;
        private readonly float FIRE_GOBLIN_RATE;
        private readonly float ELITE_GOBLIN_RATE;

        private readonly float GOBLIN_RATE_SUM;

        public Wave(int wave, float timerSpeed, int enemyNumber, float swordGoblinRate, float fireGoblinRate, float eliteGoblinRate)
        {
            WAVE = wave;
            TIMER_SPEED = timerSpeed;

            ENEMY_NUMBER = enemyNumber;
            SWORD_GOBLIN_RATE = swordGoblinRate;
            FIRE_GOBLIN_RATE = fireGoblinRate;
            ELITE_GOBLIN_RATE = eliteGoblinRate;

            GOBLIN_RATE_SUM = SWORD_GOBLIN_RATE + FIRE_GOBLIN_RATE + ELITE_GOBLIN_RATE;
        }

        public Enemy.Type RandomEnemy()
        {
            float random = Random.Range(0, GOBLIN_RATE_SUM);

            if (random < SWORD_GOBLIN_RATE)
            {
                return Enemy.Type.SwordGoblin;
            }
            random -= SWORD_GOBLIN_RATE;

            if (random < FIRE_GOBLIN_RATE)
            {
                return Enemy.Type.FireGoblin;
            }

            return Enemy.Type.EliteEnemy;
        }
    }
}
