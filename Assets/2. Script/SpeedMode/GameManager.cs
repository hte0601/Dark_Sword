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

        //balance
        private const float MAX_TIME = 100f; //시간의 최대치
        private const float BONUS_TIME_VALUE = 15f; //입력에 성공했을 때 증가하는 시간
        private const float MIN_TIME_SPEED = 20f; //게임속도 - 시작값 (단위는 1초동안 감소하는 시간의 양)
        private const float MAX_TIME_SPEED = 60f; //게임속도 - 최댓값
        private const float INCREASE_TIME_SPEED = 1f; //term마다 증가하는 게임속도
        private const float TERM_TIME_SPEED = 2f; //게임속도가 증가하는 주기 (단위 초)

        /* 고블린 간의 거리 간격이 2이고 속도가 10이므로 이론상 1초에 최대 5마리의 적을 처치할 수 있음
         * 게임속도가 60f까지 올라가고 입력에 성공했을 때 증가하는 시간이 15f이므로
         * 초당 4번 이상 입력에 성공할 경우 게임을 무한히 진행할 수 있음
         * 게임속도가 최대까지 증가하는데 1분 20초가 걸림 */


        //position
        public static Vector3 createPos = new Vector3(12f, -3.6f, 0);
        public static Vector3 battlePos = new Vector3(-7f, -3.6f, 0);

        private float timeSpeed;
        private float timeCount;

        //draw arrow
        public static int expectScore;
        public static bool isArrowDrawed;

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

        private float _timer = MAX_TIME;
        private int _currentScore = 0;
        private int _currentCombo = 0;
        private float _scoreMultiplier;

        public event Action<float> OnTimerValueChanged;
        public event Action<int> OnScoreValueChanged;
        public event Action<int, float> OnComboValueChanged;

        public float Timer
        {
            get => _timer;
            private set
            {
                if (value > MAX_TIME)
                    _timer = MAX_TIME;
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
        }

        private void Start()
        {
            notice.SetActive(false);
            StartCoroutine("StartStage");

            playdata = SaveData.instance.playData;
            EnemyManager.instance.FightEnemyEvent += HandleFightEnemyEvent;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                ExitGame();
        }

        private void HandleFightEnemyEvent(Enemy.Type enemyType, bool isInputCorrect, bool isEnemyDead)
        {
            if (isInputCorrect)
            {
                Timer += BONUS_TIME_VALUE;

                CurrentCombo += 1;
                CurrentScore += (int)(10 * ScoreMultiplier);
            }
            else
            {
                Timer = MAX_TIME;

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
                Timer -= timeSpeed * Time.deltaTime;
                timeCount += 1f * Time.deltaTime;

                if (Timer <= 0)
                {
                    GameOver();
                    yield break;
                }

                yield return null;

                //속도증가
                if (timeCount >= TERM_TIME_SPEED)
                {
                    timeCount -= TERM_TIME_SPEED;
                    if (timeSpeed < MAX_TIME_SPEED)
                        timeSpeed += INCREASE_TIME_SPEED;
                }
            }
        }

        private void Init()
        {
            Timer = MAX_TIME;
            timeSpeed = MIN_TIME_SPEED;
            timeCount = 0f;
            CurrentScore = 0;
            expectScore = 0;

            isArrowDrawed = false;
            isStart = false;

            for (int i = 0; i < 12; i++)
                EnemyManager.instance.CreateEnemy();

            CurrentCombo = 0;
            OnScoreValueChanged += OnBestScoreBroken;
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
