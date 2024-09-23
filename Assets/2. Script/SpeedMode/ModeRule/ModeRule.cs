using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class ModeRule
    {
        protected SwordmanStatus swordmanStatus;
        protected Dictionary<int, Wave> waveDataDict;


        protected ModeRule()
        {
            swordmanStatus = new();
        }


        public SwordmanStatus LoadSwordmanStatus()
        {
            return swordmanStatus;
        }

        public virtual bool LoadWaveData(int wave, out Wave waveData)
        {
            return waveDataDict.TryGetValue(wave, out waveData);
        }
    }
}
