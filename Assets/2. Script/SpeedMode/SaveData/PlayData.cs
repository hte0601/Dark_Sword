using System;

namespace SpeedMode
{
    [Serializable]
    public class PlayData : IGameSaveData
    {
        public int bestScore = 0;

        public void Save() => SaveDataManager.SaveData(this);
    }
}
