using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    [Serializable]
    public class VolumeSetting : ISaveData
    {
        public event Action<float> OnMasterVolumeChanged;
        public event Action<float> OnBGMVolumeChanged;
        public event Action<float> OnSFXVolumeChanged;
        public event Action<bool> OnMuteStateChanged;

        [SerializeField] private float _masterVolume = 0.6f;
        [SerializeField] private float _bgmVolume = 0.6f;
        [SerializeField] private float _sfxVolume = 0.6f;
        [SerializeField] private bool _isMuted = false;

        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                _masterVolume = value;
                OnMasterVolumeChanged?.Invoke(_masterVolume);
            }
        }

        public float BGMVolume
        {
            get => _bgmVolume;
            set
            {
                _bgmVolume = value;
                OnBGMVolumeChanged?.Invoke(_bgmVolume);
            }
        }

        public float SFXVolume
        {
            get => _sfxVolume;
            set
            {
                _sfxVolume = value;
                OnSFXVolumeChanged?.Invoke(_sfxVolume);
            }
        }

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                if (_isMuted != value)
                {
                    _isMuted = value;
                    OnMuteStateChanged?.Invoke(_isMuted);
                }
            }
        }

        public void Save() => SaveManager.Save<VolumeSetting>(this);
    }
}
