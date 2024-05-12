using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class RemainingEnemyUI : MonoBehaviour
    {
        [SerializeField] private Text remainingEnemyText;

        private int currentNumber = 0;

        private void Start()
        {
            UpdateRemainingEnemyText(EnemyManager.instance.RemainingEnemyNumber);
            EnemyManager.instance.OnRemainingEnemyNumberChanged += UpdateRemainingEnemyText;
        }

        private void UpdateRemainingEnemyText(int remainingEnemyNumber)
        {
            if (currentNumber == 0 && remainingEnemyNumber > 0)
            {
                StartCoroutine(IncreaseNumber(remainingEnemyNumber));
            }
            else
            {
                currentNumber = remainingEnemyNumber;
                SetRemainingEnemyText(currentNumber);
            }
        }

        private void SetRemainingEnemyText(int remainingEnemyNumber)
        {
            remainingEnemyText.text = string.Format("x {0}", remainingEnemyNumber);
        }

        private IEnumerator IncreaseNumber(int targetNumber)
        {
            // 숫자가 증가하는 중에 목표 숫자가 바뀌는 경우는 일단 고려하지 않음

            float elapsedTime = 0f;
            yield return null;

            while(true)
            {
                elapsedTime += Time.deltaTime;
                currentNumber = (int)(targetNumber * elapsedTime * 2);

                if (currentNumber >= targetNumber)
                {
                    currentNumber = targetNumber;
                    SetRemainingEnemyText(currentNumber);
                    break;
                }
                else
                {
                    SetRemainingEnemyText(currentNumber);
                }

                yield return null;
            }
        }
    }
}
