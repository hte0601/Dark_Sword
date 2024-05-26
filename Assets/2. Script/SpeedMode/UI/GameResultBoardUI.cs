using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class GameResultBoardUI : MonoBehaviour
    {
        [SerializeField] private Text gameResultText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text bestScoreText;
        [SerializeField] private Text earnedGoldText;

        public void Show(bool isGameClear, int Score, int BestScore, int EarnedGold)
        {
            gameResultText.text = isGameClear ? "Game Clear" : "Game Over";
            scoreText.text = Score.ToString();
            bestScoreText.text = BestScore.ToString();
            earnedGoldText.text = string.Format("+ {0}", EarnedGold);

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
