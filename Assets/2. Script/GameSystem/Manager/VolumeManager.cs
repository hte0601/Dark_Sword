using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace GameSystem
{
    public class VolumeManager : MonoBehaviour
    {
        private static VolumeManager instance;

        private VolumeSetting volumeSetting;
        [SerializeField] private AudioMixer mainMixer;

        private const float MUTE = 0.0001f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                volumeSetting = GameSetting.volume;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitVolume();

            volumeSetting.OnMasterVolumeChanged += SetMasterVolume;
            volumeSetting.OnBGMVolumeChanged += SetBGMVolume;
            volumeSetting.OnSFXVolumeChanged += SetSFXVolume;
            volumeSetting.OnMuteStateChanged += HandleMuteEvent;
        }

        private void InitVolume()
        {
            if (volumeSetting.IsMuted)
            {
                SetMasterVolume(MUTE);
            }
            else
            {
                SetMasterVolume(volumeSetting.MasterVolume);
            }

            SetBGMVolume(volumeSetting.BGMVolume);
            SetSFXVolume(volumeSetting.SFXVolume);
        }

        private void SetMasterVolume(float volume)
        {
            mainMixer.SetFloat("MasterMix", Mix(volume));
            volumeSetting.IsMuted = volume == MUTE;
        }

        private void SetBGMVolume(float volume)
        {
            mainMixer.SetFloat("BGMMix", Mix(volume));
        }

        private void SetSFXVolume(float volume)
        {
            mainMixer.SetFloat("SFXMix", Mix(volume));
        }

        private void HandleMuteEvent(bool isMuted)
        {
            if (isMuted)
            {
                // 음소거 시
                SetMasterVolume(MUTE);
            }
            else
            {
                // 음소거 해제 시
                if (volumeSetting.MasterVolume == MUTE)
                    volumeSetting.MasterVolume = 0.2f;
                else
                    SetMasterVolume(volumeSetting.MasterVolume);
            }
        }

        private float Mix(float volume) => Mathf.Log10(volume) * 20;
    }
}
