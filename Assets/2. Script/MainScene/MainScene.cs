using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public GameObject infoborder;
    public GameObject optionborder;
    public GameObject Battleborder;

    private void Awake()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif

#if UNITY_EDITOR
        Application.targetFrameRate = -1;
#endif
    }

    private void Start()
    {
        InitVolume();
    }

    void Update()
    {
        GameQuit();
    }

    private void InitVolume()
    {
        AudioMixer mainMixer = SoundManager.instance.mainMixer;

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

    void GameQuit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
                Application.Quit();
        }
    }

    public void TouchDownSpeedModeButton()
    {
        SceneManager.LoadScene("Speed");
    }

    public void TouchDownBattleModeButton()
    {
        Battleborder.SetActive(true);
    }

    public void BattleBorderNewButton()
    {
        BattleGameManager.DebuffReset();
        BattleBorderContinueButton();
    }

    public void BattleBorderContinueButton()
    {
        SceneManager.LoadScene("Battle_1");
    }

    public void BattleBorderExitButtion()
    {
        Battleborder.SetActive(false);
    }

    public void OptionExitButton()
    {
        optionborder.SetActive(false);
        GameSetting.volume.Save();
    }
}
