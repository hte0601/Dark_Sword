using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class GameOverBoardUI : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text bestScoreText;

        private void OnEnable()
        {
            scoreText.text = GameManager.instance.CurrentScore.ToString();
            bestScoreText.text = SaveDataManager.LoadData<PlayData>().BestScore.ToString();
        }
    }
}
