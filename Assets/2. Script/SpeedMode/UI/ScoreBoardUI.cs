using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class ScoreBoardUI : MonoBehaviour
    {
        [SerializeField] private Text killCountText;
        [SerializeField] private Text currentScoreText;
        [SerializeField] private Text bestScoreText;

        private void Start()
        {
            UpdateCurrentScoreText(GameManager.instance.CurrentScore);
            UpdateBestScoreText(GameManager.instance.BestScore);
            UpdateKillCountText(KillCounter.instance.KillCount);

            GameManager.instance.GameOverEvent += HandleGameOverEvent;
            GameManager.instance.RestartGameEvent += HandleRestartGameEvent;

            GameManager.instance.OnScoreChanged += UpdateCurrentScoreText;
            GameManager.instance.OnBestScoreChanged += UpdateBestScoreText;
            KillCounter.instance.OnKillCountChanged += UpdateKillCountText;
        }

        private void HandleGameOverEvent(bool isGameClear) => gameObject.SetActive(false);

        private void HandleRestartGameEvent() => gameObject.SetActive(true);

        private void UpdateKillCountText(int killCount)
        {
            killCountText.text = killCount.ToString();
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
