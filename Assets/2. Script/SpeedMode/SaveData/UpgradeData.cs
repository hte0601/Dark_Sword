using System;

namespace SpeedMode
{
    [Serializable]
    public class UpgradeData : IGameSaveData
    {
        // 각 업그레이드가 몇 단계인지만 저장
        public int maxHealth = 0;
        public int skillPower = 0;

        public void Save() => SaveDataManager.SaveData(this);
    }
}
