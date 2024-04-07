using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    [Serializable]
    public class UpgradeData
    {
        public const string KEY = "SpeedModeUpgradeData";

        public int maxHealth = 3;
        public int skillAutoCastNumber = 0;

        public void Save()
        {
            SaveManager.Save(KEY, this);
        }
    }
}
