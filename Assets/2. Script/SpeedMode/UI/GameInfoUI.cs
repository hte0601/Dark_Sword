using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpeedMode
{
    public class GameInfoUI : MonoBehaviour
    {
        [SerializeField] private Image gameModeFlag;
        [SerializeField] private Text currentWaveText;


        private void Awake()
        {
            SetGameModeFlag(GameMode.currentMode);
        }

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
            currentWaveText.text = string.Format("Wave {0}", wave.wave);
        }

        private void UpdateCurrentWaveText(int wave)
        {
            currentWaveText.text = string.Format("Wave {0}", wave);
        }

        private void SetGameModeFlag(GameMode.Mode? mode)
        {
            if (mode == null)
            {
                return;
            }
            else if (mode == GameMode.Mode.Test)
            {
                return;
            }
            else if (mode == GameMode.Mode.Normal)
            {
                gameModeFlag.sprite = Resources.Load<Sprite>("UI/GameModeFlag/Normal_Mode");
            }
            else if (mode == GameMode.Mode.Hard)
            {
                gameModeFlag.sprite = Resources.Load<Sprite>("UI/GameModeFlag/Hard_Mode");
            }
            else if (mode == GameMode.Mode.Infinite)
            {
                gameModeFlag.sprite = Resources.Load<Sprite>("UI/GameModeFlag/Infinite_Mode");
            }
            else
            {
                gameModeFlag.sprite = Resources.Load<Sprite>("UI/GameModeFlag/Challenge_Mode");
            }
        }
    }
}
