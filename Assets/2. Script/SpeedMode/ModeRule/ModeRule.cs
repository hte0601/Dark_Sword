using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class ModeRule
    {
        protected SwordmanStatus swordmanStatus;
        protected Dictionary<int, Wave> waveDataDict;

        protected Wave currentWaveData;
        protected Enemy.Type previousEnemy;


        protected ModeRule()
        {
            swordmanStatus = new();
        }


        public SwordmanStatus LoadSwordmanStatus()
        {
            return swordmanStatus;
        }

        public virtual bool LoadWaveData(int wave, out Wave waveData)
        {
            bool waveExist = waveDataDict.TryGetValue(wave, out waveData);

            currentWaveData = waveData;
            previousEnemy = Enemy.Type.None;

            return waveExist;
        }


        public virtual Enemy.Type RandomEnemy()
        {
            if (previousEnemy != Enemy.Type.None && Enemy.Type.CommonEnemy.HasFlag(previousEnemy))
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

        protected Enemy.Type ChooseRandomEnemy(Enemy.Type exceptEnemyType = Enemy.Type.None)
        {
            float max = currentWaveData.enemyRateSum;

            if (exceptEnemyType != Enemy.Type.None)
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

            return Enemy.Type.EliteEnemy;
        }
    }
}
