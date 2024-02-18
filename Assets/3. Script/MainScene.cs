using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public GameObject infoborder;
    public GameObject optionborder;
    public GameObject Battleborder;
    public AudioMixer mainmix;
    private bool soundOn;
    Image soundImg;
    

    [Header("Sound Slider")]
    public Slider MainVolume;
    public Slider MusicVolume;
    public Slider SFXVolume;

    private void Awake()
    {
    #if UNITY_ANDROID
            Application.targetFrameRate = 60;
    #endif

    #if UNITY_EDITOR
            Application.targetFrameRate = -1;
    #endif
    }

    void Start()
    {
        IniSound();
    }

    private void IniSound()
    {
        soundImg = GameObject.Find("SoundButton").GetComponent<Image>();

        mainmix.SetFloat("Volume", PlayerPrefs.GetFloat("Volume", 1));
        mainmix.SetFloat("BGMVolume", PlayerPrefs.GetFloat("BGMVolume", 1));
        mainmix.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 1));
        MainVolume.value = PlayerPrefs.GetFloat("Volume", 1);
        MusicVolume.value = PlayerPrefs.GetFloat("BGMVolume", 1);
        SFXVolume.value = PlayerPrefs.GetFloat("SFXVolume", 1);

        if (PlayerPrefs.GetInt("soundOn", 1) == 1)
        {
            soundOn = true;
        }
        else
        {
            mainmix.SetFloat("Volume", -80);
            soundImg.sprite = Resources.Load<Sprite>("SoundOFF") as Sprite;
            soundOn = false;
        }
    }

    void Update()
    {
        GameQuit();
    }

    void GameQuit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
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

    public void TouchDownInfoButton()
    {
        infoborder.SetActive(!infoborder.activeSelf);
    }

    public void TouchDownSoundButton()
    {
        TurnSound(!soundOn);
    }

    public void MainVolumeChange()
    {
        mainmix.SetFloat("Volume", Mathf.Log10(MainVolume.value) * 20);
        
        if(MainVolume.value == 0.0001f && soundOn)
            TurnSound(false);
        else if(MainVolume.value != 0.0001f && !soundOn)
            TurnSound(true);
    }

    public void MusicVolumeChange()
    {
        mainmix.SetFloat("BGMVolume", Mathf.Log10(MusicVolume.value) * 20);
    }

    public void SFXVolumeChange()
    {
        mainmix.SetFloat("SFXVolume", Mathf.Log10(SFXVolume.value) * 20);
    }

    public void OptionButton()
    {
        optionborder.SetActive(true);
    }

    public void OptionExitButton()
    {
        PlayerPrefs.SetFloat("Volume", MainVolume.value);
        PlayerPrefs.SetFloat("BGMVolume", MusicVolume.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume.value);
        PlayerPrefs.SetInt("soundOn", soundOn ? 1 : 0);
        PlayerPrefs.Save();

        optionborder.SetActive(false);
    }

    private void TurnSound(bool soundOnOff)
    {
        if (soundOnOff)
        {
            mainmix.SetFloat("Volume", Mathf.Log10(MainVolume.value) * 20);
            soundImg.sprite = Resources.Load<Sprite>("SoundON") as Sprite;
            soundOn = true;
            PlayerPrefs.SetInt("soundOn", 1);
        }
        else
        {
            mainmix.SetFloat("Volume", -80);
            soundImg.sprite = Resources.Load<Sprite>("SoundOFF") as Sprite;
            soundOn = false;
            PlayerPrefs.SetInt("soundOn", 0);
        }
        PlayerPrefs.Save();
    }
}
