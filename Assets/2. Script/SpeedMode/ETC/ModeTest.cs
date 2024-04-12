using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpeedMode
{
    public class ModeTest : MonoBehaviour
    {
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private Swordman swordman;
        [SerializeField] private float timeScale = 1f;

        private bool isTestMode = true;

        private void Awake()
        {
            if (!isTestMode)
            {
                gameObject.SetActive(false);
                return;
            }

#if UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
        }

        private void Update()
        {
            Time.timeScale = timeScale;

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
    }
}
