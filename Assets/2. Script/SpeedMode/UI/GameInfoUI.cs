using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class GameInfoUI : MonoBehaviour
    {
        [SerializeField] private Text CurrentWaveText;

        private void Start()
        {
            UpdateCurrentWaveText(0);
            GameManager.instance.ReadyWaveEvent += UpdateCurrentWaveText;
            GameManager.instance.RestartGameEvent += HandleRestartGameEvent;
        }

        private void HandleRestartGameEvent()
        {
            UpdateCurrentWaveText(0);
        }


        private void UpdateCurrentWaveText(Wave wave)
        {
            CurrentWaveText.text = string.Format("Wave {0}", wave.WAVE);
        }

        private void UpdateCurrentWaveText(int wave)
        {
            CurrentWaveText.text = string.Format("Wave {0}", wave);
        }
    }
}
