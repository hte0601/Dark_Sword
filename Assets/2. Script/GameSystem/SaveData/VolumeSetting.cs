using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    [Serializable]
    public class VolumeSetting : ISystemSaveData
    {
        public float masterVolume = 0.6f;
        public float bgmVolume = 0.6f;
        public float sfxVolume = 0.6f;
        public bool isMuted = false;

        public void Save() => SaveDataManager.SaveData(this);
    }
}
