using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpeedMode
{
    public class ModeTest : MonoBehaviour
    {
        [Header("GameObject")]
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private Swordman swordman;

        [Header("Test Setting")]
        [SerializeField] private bool isTestMode = false;
        [SerializeField] private GameMode.Mode gameMode = GameMode.Mode.Normal;

        [Header("Runtime Setting")]
        [SerializeField] private int frameRate = 60;
        [SerializeField] private float timeScale = 1f;

        private int currentFrameRate;
        private float currentTimeScale;


        private void Awake()
        {
            if (!isTestMode)
            {
                gameObject.SetActive(false);
                return;
            }

            GameMode.currentMode ??= gameMode;
        }

        private void Update()
        {
            SetTimeScale(timeScale);
            SetFrameRate(frameRate);

            AutoInput();
        }


        private void AutoInput()
        {
            if (!Input.GetKey(KeyCode.Z))
                return;

            Enemy enemy = enemyManager.GetHeadEnemy();

            if (enemy == null)
                return;

            if (enemy.CorrectInput == Swordman.State.Attack)
                swordman.AttackButtonInput();
            else if (enemy.CorrectInput == Swordman.State.Guard)
                swordman.GuardButtonInput();
        }

        private void SetFrameRate(int frameRate)
        {
            if (currentFrameRate != frameRate)
            {
                currentFrameRate = frameRate;
                Application.targetFrameRate = frameRate;
            }
        }

        private void SetTimeScale(float timeScale)
        {
            if (currentTimeScale != timeScale)
            {
                currentTimeScale = timeScale;
                Time.timeScale = timeScale;
            }
        }
    }
}
