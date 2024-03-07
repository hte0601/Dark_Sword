using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettingUI : MonoBehaviour
{
    [Header("Volume Slider")]
    [SerializeField] private Slider MasterVolumeSlider;
    [SerializeField] private Slider BGMVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;

    private AudioMixer mainMixer;

    private void Awake()
    {
        mainMixer = SoundManager.instance.mainMixer;

        MasterVolumeSlider.value = GameSetting.volume.MasterVolume;
        BGMVolumeSlider.value = GameSetting.volume.BGMVolume;
        SFXVolumeSlider.value = GameSetting.volume.SFXVolume;

        GameSetting.volume.OnIsMutedValueChanged += MuteEventHandler;
        MasterVolumeSlider.onValueChanged.AddListener(OnMasterSliderValueChanged);
        BGMVolumeSlider.onValueChanged.AddListener(OnBGMSliderValueChanged);
        SFXVolumeSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);
    }

    private void OnDisable()
    {
        GameSetting.volume.Save();
    }

    private void OnDestroy()
    {
        GameSetting.volume.OnIsMutedValueChanged -= MuteEventHandler;
    }

    private void OnMasterSliderValueChanged(float value)
    {
        GameSetting.volume.MasterVolume = value;
        mainMixer.SetFloat("MasterMix", GameSetting.volume.MasterMix);
    }

    private void OnBGMSliderValueChanged(float value)
    {
        GameSetting.volume.BGMVolume = value;
        mainMixer.SetFloat("BGMMix", GameSetting.volume.BGMMix);
    }

    private void OnSFXSliderValueChanged(float value)
    {
        GameSetting.volume.SFXVolume = value;
        mainMixer.SetFloat("SFXMix", GameSetting.volume.SFXMix);
    }

    private void MuteEventHandler(bool isMuted)
    {
        if (isMuted)
        {
            // 슬라이더 흐릿하게 
        }
        else
        {
            // 슬라이더 원래대로
            if (MasterVolumeSlider.value != GameSetting.volume.MasterVolume)
            {
                MasterVolumeSlider.value = GameSetting.volume.MasterVolume;
            }
        }
    }
}
