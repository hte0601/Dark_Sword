using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class ScoreBoardUI : MonoBehaviour
    {
        [SerializeField] private Text currentScoreText;
        [SerializeField] private Text bestScoreText;

        private void Start()
        {
            UpdateCurrentScoreText(GameManager.instance.CurrentScore);
            UpdateBestScoreText(SaveData.instance.playData.BestScore);

            GameManager.instance.OnScoreValueChanged += UpdateCurrentScoreText;
            SaveData.instance.playData.OnBestScoreValueChanged += UpdateBestScoreText;
        }

        private void UpdateCurrentScoreText(int currentScore)
        {
            currentScoreText.text = currentScore.ToString();
        }

        private void UpdateBestScoreText(int bestScore)
        {
            bestScoreText.text = bestScore.ToString();
        }
    }
}
