using System;

namespace SpeedMode
{
    [Serializable]
    public class ModeStatisticData : IMultipleGameSaveData
    {
        public int bestScore = 0;
        public int playNumber = 0;
        public int totalKill = 0;
        public int commonEnemyKill = 0;
        public int swordGoblinKill = 0;
        public int fireGoblinKill = 0;
        public int eliteEnemyKill = 0;
        public int spearGoblinKill = 0;


        private int _dataID;

        public int DataID
        {
            get => _dataID;
            set => _dataID = value;
        }

        public void Save() => GameSystem.SaveDataManager.SaveData(this, DataID);
    }
}
