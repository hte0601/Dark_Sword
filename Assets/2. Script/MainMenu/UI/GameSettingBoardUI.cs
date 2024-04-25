using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class GameSettingBoardUI : MonoBehaviour
    {
        private GameSystem.VolumeSetting volumeSetting;

        [Header("Volume Slider")]
        [SerializeField] private Slider MasterVolumeSlider;
        [SerializeField] private Slider BGMVolumeSlider;
        [SerializeField] private Slider SFXVolumeSlider;

        private void Awake()
        {
            volumeSetting = GameSystem.GameSetting.volume;

            MasterVolumeSlider.value = volumeSetting.MasterVolume;
            BGMVolumeSlider.value = volumeSetting.BGMVolume;
            SFXVolumeSlider.value = volumeSetting.SFXVolume;

            volumeSetting.OnMuteStateChanged += HandleMuteEvent;
            MasterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
            BGMVolumeSlider.onValueChanged.AddListener(ChangeBGMVolume);
            SFXVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        }

        private void OnDisable()
        {
            volumeSetting.Save();
        }

        private void OnDestroy()
        {
            volumeSetting.OnMuteStateChanged -= HandleMuteEvent;
        }

        private void ChangeMasterVolume(float value)
        {
            volumeSetting.MasterVolume = value;
        }

        private void ChangeBGMVolume(float value)
        {
            volumeSetting.BGMVolume = value;
        }

        private void ChangeSFXVolume(float value)
        {
            volumeSetting.SFXVolume = value;
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
                if (MasterVolumeSlider.value != volumeSetting.MasterVolume)
                {
                    MasterVolumeSlider.value = volumeSetting.MasterVolume;
                }
            }
        }

        public void OnExitButtonPointerDown()
        {
            BoardUIManager.instance.CloseBoardUI(gameObject);
        }
    }
}
