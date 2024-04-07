using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpeedMode
{
    public class ModeTest : MonoBehaviour
    {
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private Swordman swordman;

        private Enemy enemy;

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
            if (Input.GetKey(KeyCode.Z))
            {
                enemy = enemyManager.GetHeadEnemy();

                if (enemy != null)
                    swordman.HandleInput(enemy.CorrectInput);
            }
        }
    }
}
