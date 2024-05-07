using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    // 게임 오브젝트가 활성화된 상태로 씬에 배치되어 있어야 함
    public class GoldUI : MonoBehaviour
    {
        [SerializeField] private Text goldText;

        private void Awake()
        {
            GameSystem.CurrencyManager.OnGoldValueChanged += UpdateGoldText;
        }

        private void Start()
        {
            GameManager.instance.GameOverEvent += HandleGameOverEvent;
            GameManager.instance.RestartGameEvent += HandleRestartGameEvent;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameSystem.CurrencyManager.OnGoldValueChanged -= UpdateGoldText;
        }

        private void HandleGameOverEvent() => gameObject.SetActive(true);

        private void HandleRestartGameEvent() => gameObject.SetActive(false);

        private void UpdateGoldText(int gold)
        {
            goldText.text = gold.ToString();
        }
    }
}
