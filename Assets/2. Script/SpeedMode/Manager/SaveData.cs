using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SaveData : MonoBehaviour
    {
        public static SaveData instance;
        public PlayData playData;
        public UpgradeData upgrades;

        private void Awake()
        {
            instance = this;
            playData = GameSystem.SaveManager.Load<PlayData>(PlayData.KEY);
            upgrades = GameSystem.SaveManager.Load<UpgradeData>(UpgradeData.KEY);
        }
    }
}
