using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class ModeRule
    {
        protected Wave currentWave;
        protected Enemy.Type previousEnemy;

        protected Dictionary<int, Wave> waveDict;


        protected ModeRule() {}

        public virtual bool LoadWaveData(int wave, out Wave waveData)
        {
            bool waveExist = waveDict.TryGetValue(wave, out waveData);

            currentWave = waveData;
            previousEnemy = Enemy.Type.None;

            return waveExist;
        }

        public virtual Enemy.Type RandomEnemy()
        {
            if (previousEnemy != Enemy.Type.None && Enemy.Type.CommonEnemy.HasFlag(previousEnemy))
            {
                // 연속성이 양수이고 확률이 발동한 경우
                if (currentWave.continuity > Tool.Random.Value())
                {
                    return previousEnemy;
                }
                // 연속성이 음수이고 확률이 발동한 경우
                else if (-currentWave.continuity > Tool.Random.Value())
                {
                    previousEnemy = RandomEnemyType(previousEnemy);
                    return previousEnemy;
                }
            }

            previousEnemy = RandomEnemyType();
            return previousEnemy;
        }

        protected Enemy.Type RandomEnemyType(Enemy.Type exceptEnemyType = Enemy.Type.None)
        {
            float max = currentWave.enemyRateSum;

            if (exceptEnemyType != Enemy.Type.None)
                max -= currentWave.enemyRateDict[exceptEnemyType];

            float randomValue = Tool.Random.Range(0f, max);

            foreach (var item in currentWave.enemyRateDict)
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

            return Enemy.Type.EliteEnemy;
        }
    }
}
