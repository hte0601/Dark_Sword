using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetting
{
    public static VolumeSetting volume;

    // 정적 생성자
    static GameSetting()
    {
        volume = SaveManager.Load<VolumeSetting>(VolumeSetting.KEY);
    }
}

[Serializable]
public class VolumeSetting
{
    public const string KEY = "VolumeSetting";
    [SerializeField] private float masterVolume = 0.6f;
    [SerializeField] private float bgmVolume = 0.6f;
    [SerializeField] private float sfxVolume = 0.6f;
    [SerializeField] private bool isMuted = false;
    public event Action<bool> OnIsMutedValueChanged;

    public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = value;
            IsMuted = masterVolume == 0.0001f;
        }
    }

    public float BGMVolume
    {
        get => bgmVolume;
        set => bgmVolume = value;
    }

    public float SFXVolume
    {
        get => sfxVolume;
        set => sfxVolume = value;
    }

    public float MasterMix
    {
        get => Mathf.Log10(masterVolume) * 20;
    }

    public float BGMMix
    {
        get => Mathf.Log10(bgmVolume) * 20;
    }

    public float SFXMix
    {
        get => Mathf.Log10(sfxVolume) * 20;
    }

    public bool IsMuted
    {
        get => isMuted;
        set
        {
            if (isMuted != value)
            {
                isMuted = value;
                OnIsMutedValueChanged?.Invoke(isMuted);
            }
        }
    }

    public void Save()
    {
        SaveManager.Save(KEY, this);
    }
}