using System;

namespace SpeedMode
{
    [Serializable]
    public class StatisticData : IGameSaveData
    {
        public int playNumber = 0;
        public int totalKill = 0;
        public int commonEnemyKill = 0;
        public int swordGoblinKill = 0;
        public int fireGoblinKill = 0;
        public int eliteEnemyKill = 0;
        public int spearGoblinKill = 0;

        public void Save() => SaveDataManager.SaveData(this);
    }
}
