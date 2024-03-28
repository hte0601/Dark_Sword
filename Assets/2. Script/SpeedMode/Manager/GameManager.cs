using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace SpeedMode
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        //other value
        public static bool isStart;

        //GameOver
        public GameObject notice;
        public GameObject scoreBoard;
        public Text scoreText;
        public Text bestScoreText;

        //Guide UI
        public GameObject guide;

        private PlayData playdata;
        private Wave currentWave;

        private float _timer;
        private int _currentScore = 0;
        private int _currentCombo = 0;
        private float _scoreMultiplier;

        public event Action<float> OnTimerValueChanged;
        public event Action<int> OnScoreValueChanged;
        public event Action<int, float> OnComboValueChanged;

        public event Action<int> ReadyWaveEvent;
        public event Action<int> StartWaveEvent;
        public event Action<int> EndWaveEvent;

        public float Timer
        {
            get => _timer;
            private set
            {
                if (value > ModeData.TimerData.MAX_TIME)
                    _timer = ModeData.TimerData.MAX_TIME;
                else
                    _timer = value;

                OnTimerValueChanged?.Invoke(_timer);
            }
        }

        public int CurrentScore
        {
            get => _currentScore;
            private set
            {
                _currentScore = value;
                OnScoreValueChanged?.Invoke(_currentScore);
            }
        }

        public int CurrentCombo
        {
            get => _currentCombo;
            private set
            {
                _currentCombo = value;
                ScoreMultiplier = _currentCombo;
                OnComboValueChanged?.Invoke(_currentCombo, ScoreMultiplier);
            }
        }

        public float ScoreMultiplier
        {
            get => _scoreMultiplier;
            private set => _scoreMultiplier = 1 + (int)(value / 100) * 0.1f;
        }


        private void Awake()
        {
            instance = this;

            currentWave = ModeData.WaveData.waves[8];  // 임시
        }

        private void Start()
        {
            notice.SetActive(false);
            StartCoroutine("StartStage");

            playdata = SaveData.instance.playData;
            EnemyManager.instance.BattleEnemyEvent += HandleBattleEnemyEvent;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                ExitGame();
        }

        private void RaiseReadyWaveEvent(int wave)
        {
            currentWave = ModeData.WaveData.waves[wave];
            ReadyWaveEvent?.Invoke(wave);
        }

        private void RaiseStartWaveEvent(int wave)
        {
            StartWaveEvent?.Invoke(wave);
        }

        public void RaiseEndWaveEvent(int wave)
        {
            EndWaveEvent?.Invoke(wave);
        }


        private void HandleBattleEnemyEvent(Enemy.Type enemyType, bool isInputCorrect, bool isEnemyDead)
        {
            if (isInputCorrect)
            {
                Timer += ModeData.TimerData.ADDITIONAL_TIME;

                CurrentCombo += 1;
                CurrentScore += (int)(10 * ScoreMultiplier);
            }
            else
            {
                // 타이머 회복 코드

                CurrentCombo = 0;
            }
        }

        private void OnBestScoreBroken(int currentScore)
        {
            if (currentScore > playdata.BestScore)
            {
                SoundManager.PlayBestScoreUpdate();
                OnScoreValueChanged -= OnBestScoreBroken;
            }
        }



        IEnumerator StartStage()
        {
            Init();
            while (!isStart)
                yield return null;

            while (true)
            {
                //timer down
                Timer -= currentWave.TIMER_SPEED * Time.deltaTime;

                if (Timer <= 0)
                {
                    GameOver();
                    yield break;
                }

                yield return null;
            }
        }

        private void Init()
        {
            Timer = ModeData.TimerData.MAX_TIME;
            CurrentScore = 0;
            CurrentCombo = 0;
            isStart = false;

            OnScoreValueChanged += OnBestScoreBroken;

            for (int i = 0; i < 12; i++)
                EnemyManager.instance.CreateEnemy();
        }

        // public static void TimeUp()
        // {
        //     time.value += BONUS_TIME_VALUE;
        //     score += 1;
        //     GM.nowScore.text = score.ToString();
        // }

        public static void GameOver()
        {
            // Swordman.setPlayerState(4);

            if (instance.CurrentScore > instance.playdata.BestScore)
            {
                instance.playdata.BestScore = instance.CurrentScore;
                instance.playdata.Save();
            }

            instance.bestScoreText.text = instance.playdata.BestScore.ToString();
            instance.scoreText.text = instance.CurrentScore.ToString();
            instance.scoreBoard.SetActive(false);
            instance.notice.SetActive(true);

            SoundManager.PlayGameOverSound();
            ParticleManager.CreateBrokenHeartParticle();
        }

        public void RestartGame()
        {
            EnemyManager.ClearEnemy();
            notice.SetActive(false);
            scoreBoard.SetActive(true);
            // Swordman.setPlayerState(0);
            SoundManager.BGMStart();
            StartCoroutine("StartStage");
        }

        public void ExitGame()
        {
            EnemyManager.ClearEnemy();
            SceneManager.LoadScene("Main");
            notice.SetActive(false);
        }

        public void HideGuide()
        {
            guide.SetActive(false);
        }
    }
}
