using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace SpeedMode
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public event Action<int> ReadyWaveEvent;
        public event Action<int> StartWaveEvent;
        public event Action<int> EndWaveEvent;

        public event Action GameOverEvent;
        public event Action RestartGameEvent;

        public event Action<float> OnTimerValueChanged;
        public event Action<int> OnScoreValueChanged;
        public event Action<int, float> OnComboValueChanged;
        public event Action<int> OnKillCountValueChanged;

        [SerializeField] private GameObject gameOverBoard;
        private Swordman swordman;
        private PlayData playdata;
        private Wave currentWave;

        private float _timer;
        private bool isTimerWaitingInput = true;
        private bool isTimerStopped = true;
        private bool isGamePaused = false;
        private int _currentScore = 0;
        private int _currentCombo = 0;
        private float _scoreMultiplier;
        private int _killCount = 0;

        public float Timer
        {
            get => _timer;
            private set
            {
                if (value > ModeData.TimerData.MAX_TIME)
                    _timer = ModeData.TimerData.MAX_TIME;
                else if (value < 0f)
                    _timer = 0f;
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

        public int KillCount
        {
            get => _killCount;
            private set
            {
                _killCount = value;
                OnKillCountValueChanged?.Invoke(_killCount);
            }
        }


        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Initialize();
            StartCoroutine(TimerCoroutine());

            swordman = Swordman.instance;
            swordman.BattleEnemyEvent += HandleBattleEnemyEvent;
            playdata = SaveDataManager.LoadData<PlayData>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                ExitGame();
        }

        private void Initialize()
        {
            Timer = ModeData.TimerData.MAX_TIME;
            isTimerWaitingInput = true;
            CurrentScore = 0;
            CurrentCombo = 0;
            KillCount = 0;

            // 베스트 스코어를 갱신하지 못 하고 게임이 다시 시작되면
            // OnBestScoreBroken이 두 번 등록되는 문제가 있음
            OnScoreValueChanged -= OnBestScoreBroken;
            OnScoreValueChanged += OnBestScoreBroken;

            StartCoroutine(WaitAndInvoke(1f, RaiseReadyWaveEvent, 1));
        }


        private void RaiseReadyWaveEvent(int wave)
        {
            Debug.Log(string.Format("{0}웨이브 준비", wave));
            currentWave = ModeData.WaveData.waves[wave];

            ReadyWaveEvent?.Invoke(wave);
            StartCoroutine(WaitAndInvoke(1f, RaiseStartWaveEvent, wave));
        }

        private void RaiseStartWaveEvent(int wave)
        {
            Debug.Log(string.Format("{0}웨이브 시작", wave));
            isTimerStopped = false;
            isTimerWaitingInput = true;
            StartWaveEvent?.Invoke(wave);
        }

        public void RaiseEndWaveEvent(int wave)
        {
            // 코루틴으로 실행을 잠깐 지연시켜야 함
            Debug.Log(string.Format("{0}웨이브 종료", wave));
            isTimerStopped = true;
            StartCoroutine(RestoreTimer());

            EndWaveEvent?.Invoke(wave);
            StartCoroutine(WaitAndInvoke(1f, RaiseReadyWaveEvent, wave + 1));
        }

        private void RaiseGameOverEvent()
        {
            // Swordman.setPlayerState(4);

            if (CurrentScore > playdata.BestScore)
            {
                playdata.BestScore = CurrentScore;
                playdata.Save();
            }

            SoundManager.PlayGameOverSound();
            ParticleManager.CreateBrokenHeartParticle();

            GameOverEvent?.Invoke();
            gameOverBoard.SetActive(true);
        }

        // 버튼 이벤트에 연결됨
        public void RaiseRestartGameEvent()
        {
            gameOverBoard.SetActive(false);
            // Swordman.setPlayerState(0);
            SoundManager.BGMStart();
            Initialize();

            RestartGameEvent?.Invoke();
        }


        private void HandleBattleEnemyEvent(BattleReport battleReport)
        {
            if (battleReport.result == BattleReport.Result.InputCorrect)
            {
                isTimerWaitingInput = false;

                Timer += ModeData.TimerData.ADDITIONAL_TIME;
                CurrentCombo += 1;
                CurrentScore += (int)(10 * ScoreMultiplier);

                if (battleReport.isEnemyDead)
                    KillCount += 1;
            }
            else if (battleReport.result == BattleReport.Result.SkillHit)
            {
                for (int i = 0; i < battleReport.damageDealt; i++)
                {
                    Timer += ModeData.TimerData.ADDITIONAL_TIME;
                    CurrentCombo += 1;
                    CurrentScore += (int)(10 * ScoreMultiplier);
                }

                KillCount += 1;
            }
            else if (battleReport.result == BattleReport.Result.SkillCast)
            {
                isTimerWaitingInput = true;
            }
            else
            {
                OnSwordmanTakeDamage(battleReport.result);
            }
        }

        private void OnSwordmanTakeDamage(BattleReport.Result result)
        {
            if (result == BattleReport.Result.SwordmanGroggy)
            {
                isTimerWaitingInput = true;
                StartCoroutine(RestoreTimer());

                CurrentCombo = 0;
            }
            else if (result == BattleReport.Result.GameOver)
            {
                isTimerStopped = true;
                RaiseGameOverEvent();
            }
            else if (result == BattleReport.Result.SkillAutoCast)
            {
                isTimerWaitingInput = true;
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


        private IEnumerator TimerCoroutine()
        {
            while (true)
            {
                while (isTimerWaitingInput || isTimerStopped || isGamePaused)
                {
                    yield return null;
                }

                Timer -= currentWave.TIMER_SPEED * Time.deltaTime;

                if (Timer == 0)
                    OnSwordmanTakeDamage(swordman.TakeDamage());

                yield return null;
            }
        }

        private IEnumerator RestoreTimer()
        {
            yield return new WaitForSeconds(1f);

            while (Timer < ModeData.TimerData.MAX_TIME)
            {
                Timer += ModeData.TimerData.MAX_TIME * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator WaitAndInvoke<T>(float waitSeconds, Action<T> function, T arg)
        {
            yield return new WaitForSeconds(waitSeconds);

            function.Invoke(arg);
        }

        // 버튼 이벤트에 연결됨
        public void ExitGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void HideGuide()
        {
            gameOverBoard.SetActive(false);
        }
    }
}
