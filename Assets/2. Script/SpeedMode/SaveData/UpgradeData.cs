using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    [Serializable]
    public class UpgradeData : ISaveData
    {
        public int maxHealth = 3;
        public int skillPower = 8;
        public int skillAutoCastNumber = 0;

        public void Save() => GameSystem.SaveManager.Save<UpgradeData>(this);
    }
}
