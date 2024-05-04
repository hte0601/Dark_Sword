using System;

namespace SpeedMode
{
    [Serializable]
    public class UpgradeData : IGameSaveData
    {
        public int maxHealth = 3;
        public int skillPower = 8;
        public int skillAutoCastNumber = 0;

        public void Save() => SaveDataManager.SaveData(this);
    }
}
