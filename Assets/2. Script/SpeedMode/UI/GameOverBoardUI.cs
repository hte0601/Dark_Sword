using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class GameOverBoardUI : MonoBehaviour
    {
        private GameManager gameManager;

        [SerializeField] private Text scoreText;
        [SerializeField] private Text bestScoreText;

        private void Awake()
        {
            gameManager = GameManager.instance;
        }

        private void OnEnable()
        {
            scoreText.text = gameManager.CurrentScore.ToString();
            bestScoreText.text = gameManager.BestScore.ToString();
        }
    }
}
