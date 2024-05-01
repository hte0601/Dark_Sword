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
            GameManager gameManager = GameManager.instance;
            PlayData playData = SaveDataManager.LoadData<PlayData>();

            UpdateKillCountText(gameManager.KillCount);
            UpdateCurrentScoreText(gameManager.CurrentScore);
            UpdateBestScoreText(playData.BestScore);

            gameManager.GameOverEvent += HandleGameOverEvent;
            gameManager.RestartGameEvent += HandleRestartGameEvent;

            gameManager.OnKillCountValueChanged += UpdateKillCountText;
            gameManager.OnScoreValueChanged += UpdateCurrentScoreText;
            playData.OnBestScoreValueChanged += UpdateBestScoreText;
        }

        private void HandleGameOverEvent() => gameObject.SetActive(false);

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
