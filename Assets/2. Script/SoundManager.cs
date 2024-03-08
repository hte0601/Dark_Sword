using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer mainMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitVolume();
    }

    private void InitVolume()
    {
        if (GameSetting.volume.IsMuted)
        {
            mainMixer.SetFloat("MasterMix", -80);
        }
        else
        {
            mainMixer.SetFloat("MasterMix", GameSetting.volume.MasterMix);
        }
        mainMixer.SetFloat("BGMMix", GameSetting.volume.BGMMix);
        mainMixer.SetFloat("SFXMix", GameSetting.volume.SFXMix);
    }
}
