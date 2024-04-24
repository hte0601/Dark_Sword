using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    [Serializable]
    public class PlayData
    {
        public const string KEY = "SpeedModePlayData";

        public event Action<int> OnBestScoreValueChanged;

        [SerializeField] private int _bestScore = 0;
        public int playNumber = 0;
        public int totalKill = 0;
        public int commonEnemyKill = 0;
        public int swordGoblinKill = 0;
        public int fireGoblinKill = 0;
        public int eliteEnemyKill = 0;
        public int spearGoblinKill = 0;

        public int BestScore
        {
            get => _bestScore;
            set
            {
                _bestScore = value;
                OnBestScoreValueChanged?.Invoke(_bestScore);
            }
        }

        public void Save()
        {
            GameSystem.SaveManager.Save(KEY, this);
        }
    }
}
