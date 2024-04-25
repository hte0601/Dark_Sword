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
            playData = GameSystem.SaveManager.Load<PlayData>();
            upgrades = GameSystem.SaveManager.Load<UpgradeData>();
        }
    }
}
