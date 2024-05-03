using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace GameSystem
{
    public class VolumeManager : MonoBehaviour
    {
        public static VolumeManager instance;

        // public event Action<float> OnMasterVolumeChanged;
        // public event Action<float> OnBGMVolumeChanged;
        // public event Action<float> OnSFXVolumeChanged;
        public event Action<bool> OnMuteStateChanged;

        [SerializeField] private AudioMixer mainMixer;
        private VolumeSetting volumeSetting;

        private const float MUTE = 0.0001f;

        public float MasterVolume
        {
            get => volumeSetting.masterVolume;
            set
            {
                volumeSetting.masterVolume = value;
                SetMasterVolume(value);
                IsMuted = value == MUTE;
            }
        }

        public float BGMVolume
        {
            get => volumeSetting.bgmVolume;
            set
            {
                volumeSetting.bgmVolume = value;
                SetBGMVolume(value);
            }
        }

        public float SFXVolume
        {
            get => volumeSetting.sfxVolume;
            set
            {
                volumeSetting.sfxVolume = value;
                SetSFXVolume(value);
            }
        }

        public bool IsMuted
        {
            get => volumeSetting.isMuted;
            set
            {
                if (volumeSetting.isMuted == value)
                    return;

                volumeSetting.isMuted = value;
                SaveVolumeSetting();

                if (value)
                {
                    // 음소거 시
                    SetMasterVolume(MUTE);
                }
                else
                {
                    // 음소거 해제 시
                    if (MasterVolume == MUTE)
                        MasterVolume = 0.2f;
                    else
                        SetMasterVolume(MasterVolume);
                }

                OnMuteStateChanged?.Invoke(value);
            }
        }


        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);

            instance = this;
            DontDestroyOnLoad(gameObject);

            volumeSetting = SaveDataManager.LoadData<VolumeSetting>();
        }

        private void Start()
        {
            InitVolume();
        }

        private void InitVolume()
        {
            SetMasterVolume(IsMuted ? MUTE : MasterVolume);
            SetBGMVolume(BGMVolume);
            SetSFXVolume(SFXVolume);
        }


        public void SaveVolumeSetting()
        {
            volumeSetting.Save();
        }


        private float Mix(float volume) => Mathf.Log10(volume) * 20;
        private void SetMasterVolume(float volume) => mainMixer.SetFloat("MasterMix", Mix(volume));
        private void SetBGMVolume(float volume) => mainMixer.SetFloat("BGMMix", Mix(volume));
        private void SetSFXVolume(float volume) => mainMixer.SetFloat("SFXMix", Mix(volume));
    }
}
