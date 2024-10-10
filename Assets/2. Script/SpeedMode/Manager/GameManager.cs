using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace SpeedMode
{
    public class GameManager : MonoBehaviourExt
    {
        public static GameManager instance;

        public event Action<Wave> ReadyWaveEvent;
        public event Action<int> StartWaveEvent;
        public event Action<int> EndWaveEvent;

        public event Action<bool> GameOverEvent;
        public event Action RestartGameEvent;

        public event Action<float> OnTimerChanged;
        public event Action<int> OnScoreChanged;
        public event Action<int> OnBestScoreChanged;
        public event Action<int, float> OnComboChanged;

        [SerializeField] private GameResultBoardUI gameResultBoard;
        private Swordman swordman;
        private KillCounter killCounter;
        private ModeStatisticData statisticData;
        private ModeData modeData;
        private Wave currentWaveData;

        private float _timer;
        private bool isTimerWaitingInput = true;
        private bool isTimerStopped = true;
        private bool isGamePaused = false;
        private int _currentScore = 0;
        private int _currentCombo = 0;
        private float _scoreMultiplier;

        public float Timer
        {
            get => _timer;
            private set
            {
                if (value > GameData.TimerData.MAX_TIME)
                    _timer = GameData.TimerData.MAX_TIME;
                else if (value < 0f)
                    _timer = 0f;
                else
                    _timer = value;

                OnTimerChanged?.Invoke(_timer);
            }
        }

        public int CurrentScore
        {
            get => _currentScore;
            private set
            {
                _currentScore = value;
                OnScoreChanged?.Invoke(_currentScore);
            }
        }

        public int BestScore
        {
            get => statisticData.bestScore;
            private set
            {
                statisticData.bestScore = value;
                statisticData.Save();
                OnBestScoreChanged?.Invoke(statisticData.bestScore);
            }
        }

        public int CurrentCombo
        {
            get => _currentCombo;
            private set
            {
                _currentCombo = value;
                ScoreMultiplier = _currentCombo;
                OnComboChanged?.Invoke(_currentCombo, ScoreMultiplier);
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
            statisticData = SaveDataManager.LoadData<ModeStatisticData>((int)GameMode.currentMode);
        }

        private void Start()
        {
            Initialize();
            StartCoroutine(TimerCoroutine());

            swordman = Swordman.instance;
            killCounter = KillCounter.instance;
            modeData = GameMode.instance.modeData;

            swordman.BattleEnemyEvent += HandleBattleEnemyEvent;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                ExitGame();
        }

        private void Initialize()
        {
            Timer = GameData.TimerData.MAX_TIME;
            isTimerWaitingInput = true;
            CurrentScore = 0;
            CurrentCombo = 0;

            // 베스트 스코어를 갱신하지 못 하고 게임이 다시 시작되면
            // OnBestScoreBroken이 두 번 등록되는 문제가 있음
            OnScoreChanged -= OnBestScoreBroken;
            OnScoreChanged += OnBestScoreBroken;

            DelayInvoke(1f, RaiseReadyWaveEvent, 1);
        }


        private void RaiseReadyWaveEvent(int wave)
        {
            if (modeData.LoadWaveData(wave, out currentWaveData))
            {
                Debug.Log(string.Format("{0}웨이브 준비", wave));

                ReadyWaveEvent?.Invoke(currentWaveData);
                DelayInvoke(1f, RaiseStartWaveEvent, wave);
            }
            else
            {
                RaiseGameOverEvent(true);
            }
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
            DelayInvoke(1f, RaiseReadyWaveEvent, wave + 1);
        }

        private void RaiseGameOverEvent(bool isGameClear)
        {
            SoundManager.StopBGM();

            if (BestScore < CurrentScore)
                BestScore = CurrentScore;

            int earnedGold = killCounter.GetKillCount(Enemy.Types.CommonEnemy) / 5
                + killCounter.GetKillCount(Enemy.Types.EliteEnemy) / 2;

            GameSystem.CurrencyManager.IncreaseGold(earnedGold);

            GameOverEvent?.Invoke(isGameClear);
            gameResultBoard.Show(isGameClear, CurrentScore, BestScore, earnedGold);
        }

        // 버튼 이벤트에 연결됨
        public void RaiseRestartGameEvent()
        {
            gameResultBoard.Hide();

            Initialize();
            SoundManager.PlayBGM();

            RestartGameEvent?.Invoke();
        }


        private void HandleBattleEnemyEvent(BattleReport battleReport)
        {
            if (battleReport.result == BattleReport.Result.InputCorrect)
            {
                isTimerWaitingInput = false;

                Timer += GameData.TimerData.ADDITIONAL_TIME;
                CurrentCombo += 1;
                CurrentScore += (int)(10 * ScoreMultiplier);
            }
            else if (battleReport.result == BattleReport.Result.SkillHit)
            {
                for (int i = 0; i < battleReport.dealtDamage; i++)
                {
                    Timer += GameData.TimerData.ADDITIONAL_TIME;
                    CurrentCombo += 1;
                    CurrentScore += (int)(10 * ScoreMultiplier);
                }
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
                RaiseGameOverEvent(false);
            }
        }

        private void OnBestScoreBroken(int currentScore)
        {
            if (currentScore > BestScore)
            {
                SoundManager.PlaySFX(SFX.Game.BestScoreUpdate);
                OnScoreChanged -= OnBestScoreBroken;
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

                Timer -= currentWaveData.timerSpeed * Time.deltaTime;

                if (Timer == 0)
                    OnSwordmanTakeDamage(swordman.TakeDamage());

                yield return null;
            }
        }

        private IEnumerator RestoreTimer()
        {
            yield return new WaitForSeconds(1f);

            while (Timer < GameData.TimerData.MAX_TIME)
            {
                Timer += GameData.TimerData.MAX_TIME * Time.deltaTime;
                yield return null;
            }
        }

        // 버튼 이벤트에 연결됨
        public void ExitGame()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
