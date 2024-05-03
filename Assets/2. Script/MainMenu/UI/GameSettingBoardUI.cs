using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class GameSettingBoardUI : MonoBehaviour
    {
        private GameSystem.VolumeManager volumeManager;

        [Header("Volume Slider")]
        [SerializeField] private Slider MasterVolumeSlider;
        [SerializeField] private Slider BGMVolumeSlider;
        [SerializeField] private Slider SFXVolumeSlider;

        private void Awake()
        {
            volumeManager = GameSystem.VolumeManager.instance;

            MasterVolumeSlider.value = volumeManager.MasterVolume;
            BGMVolumeSlider.value = volumeManager.BGMVolume;
            SFXVolumeSlider.value = volumeManager.SFXVolume;

            volumeManager.OnMuteStateChanged += HandleMuteEvent;
            MasterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
            BGMVolumeSlider.onValueChanged.AddListener(ChangeBGMVolume);
            SFXVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        }

        private void OnDisable()
        {
            volumeManager.SaveVolumeSetting();
        }

        private void OnDestroy()
        {
            volumeManager.OnMuteStateChanged -= HandleMuteEvent;
        }

        private void ChangeMasterVolume(float value)
        {
            volumeManager.MasterVolume = value;
        }

        private void ChangeBGMVolume(float value)
        {
            volumeManager.BGMVolume = value;
        }

        private void ChangeSFXVolume(float value)
        {
            volumeManager.SFXVolume = value;
        }

        private void HandleMuteEvent(bool isMuted)
        {
            if (isMuted)
            {
                // 슬라이더 흐릿하게
            }
            else
            {
                // 슬라이더 원래대로
                if (MasterVolumeSlider.value != volumeManager.MasterVolume)
                {
                    MasterVolumeSlider.value = volumeManager.MasterVolume;
                }
            }
        }

        public void OnExitButtonPointerDown()
        {
            BoardUIManager.instance.CloseBoardUI(gameObject);
        }
    }
}
