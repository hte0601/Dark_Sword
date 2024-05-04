using System;

namespace GameSystem
{
    [Serializable]
    public class CurrencyData : ISystemSaveData
    {
        public int gold = 0;

        public void Save() => SaveDataManager.SaveData(this);
    }
}
