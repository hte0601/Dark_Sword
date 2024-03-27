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

        [SerializeField] private int _bestScore = 0;

        public event Action<int> OnBestScoreValueChanged;

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
            SaveManager.Save(KEY, this);
        }
    }
}
