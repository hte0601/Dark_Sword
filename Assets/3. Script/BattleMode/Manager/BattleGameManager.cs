using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class BattleGameManager : MonoBehaviour
{
    public static BattleGameManager gameManager;
    public static string DeviceType;
    public GameObject GameOverUI;
    public GameObject VictoryUI;
    public GameObject GuideUI;
    public GameObject DebuffSelectUI;
    public GameObject DebuffList;
    public AudioMixerGroup BGM;

    //각 디버프가 몇 스택씩 쌓였는지 저장
    //디버프 수치는 스택 * 계수 하여 property에서 리턴
    private int debuffCount;
    private int debuffBossDamageUp;
    private int debuffBossSpeedUp;
    private int debuffPerfectDefenseTimeDown;
    private int debuffStaminaRegenDown;
    private int debuffBossStartPhase;
    private AudioSource[] audiosources;

    //1 : dmg, 2 : spd, 3 : defense, 4 : stamina, 5 : phase
    [Header("Debuff Image")]
    public GameObject[] Debuff;
    private int[] selectedImg;

    void Start()
    {
        gameManager = this;
        audiosources = gameObject.GetComponents<AudioSource>();

        Load();
        if(debuffCount == 0)
        {
            VolumeOff();
            Time.timeScale = 0f;
            GuideUI.SetActive(true);
        }
        
        if(SystemInfo.deviceType == UnityEngine.DeviceType.Desktop)
            DeviceType = "PC";
        else if(SystemInfo.deviceType == UnityEngine.DeviceType.Handheld)
            DeviceType = "Mobile";
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }
    }

    void Save()
    {
        PlayerPrefs.SetInt("DebuffCount", debuffCount);
        PlayerPrefs.SetInt("DebuffBossDamageUp", debuffBossDamageUp);
        PlayerPrefs.SetInt("DebuffBossSpeedUp", debuffBossSpeedUp);
        PlayerPrefs.SetInt("DebuffPerfectDefenseTimeDown", debuffPerfectDefenseTimeDown);
        PlayerPrefs.SetInt("DebuffStaminaRegenDown", debuffStaminaRegenDown);
        PlayerPrefs.SetInt("DebuffBossStartPhase", debuffBossStartPhase);
        PlayerPrefs.Save();
    }

    void Load()
    {
        debuffCount = PlayerPrefs.GetInt("DebuffCount", 0);
        debuffBossDamageUp = PlayerPrefs.GetInt("DebuffBossDamageUp", 0);
        debuffBossSpeedUp = PlayerPrefs.GetInt("DebuffBossSpeedUp", 0);
        debuffPerfectDefenseTimeDown = PlayerPrefs.GetInt("DebuffPerfectDefenseTimeDown", 0);
        debuffStaminaRegenDown = PlayerPrefs.GetInt("DebuffStaminaRegenDown", 0);
        debuffBossStartPhase = PlayerPrefs.GetInt("DebuffBossStartPhase", 1);

        Text[] texts = DebuffList.GetComponentsInChildren<Text>();
        texts[0].text = "보스 공격력 : + " + DebuffBossDamageUp;
        texts[1].text = "보스 속도 : + " + DebuffBossSpeedUp * 100 + "%";
        texts[2].text = "완벽 가드 시간 : - " + DebuffPerfectDefenseTimeDown + "초";
        texts[3].text = "스태미나 회복속도 : - " + DebuffStaminaRegenDown * 3 + "%";
        if(DebuffBossStartPhase == 2)
            texts[4].text = "2페이즈 시작";
    }

    public static void DebuffReset()
    {
        PlayerPrefs.DeleteKey("DebuffCount");
        PlayerPrefs.DeleteKey("DebuffBossDamageUp");
        PlayerPrefs.DeleteKey("DebuffBossSpeedUp");
        PlayerPrefs.DeleteKey("DebuffPerfectDefenseTimeDown");
        PlayerPrefs.DeleteKey("DebuffStaminaRegenDown");
        PlayerPrefs.DeleteKey("DebuffBossStartPhase");
    }

    public static void GameOver()
    {
        gameManager.GameOverUI.SetActive(true);
    }

    public static void Victory()
    {
        //gameManager.VictoryUI.SetActive(true);
        gameManager.DebuffSelectUI.SetActive(true);
        gameManager.SelectDebuff();
    }

    private void SelectDebuff()
    {
        int index;
        selectedImg = new int[3];
        List<int> debuffList = new List<int>();
        System.Random rand = new System.Random();

        if (debuffBossDamageUp < 2)
            debuffList.Add(0);
        if (debuffBossSpeedUp < 2)
            debuffList.Add(1);
        if (debuffPerfectDefenseTimeDown < 2)
            debuffList.Add(2);
        if (debuffStaminaRegenDown < 2)
            debuffList.Add(3);
        if (debuffBossStartPhase < 2)
            debuffList.Add(4);

        for(int i = 0; i < 3; i++)
        {
            if(debuffList.Count > 0)
            {
                index = rand.Next(debuffList.Count);
                selectedImg[i] = debuffList[index];
                debuffList.RemoveAt(index);
            }
            else
            {
                selectedImg[i] = -1;
            }
        }

        gameManager.ShowDebuff();
    }

    private void ShowDebuff()
    {
        Image[] backgroundImgs = DebuffSelectUI.GetComponentsInChildren<Image>();
        Text[] texts = DebuffSelectUI.GetComponentsInChildren<Text>();
        for(int i = 0; i < 3; i++)
        {
            if(selectedImg[i] == -1)
                continue;
            texts[i].text = DebuffText(selectedImg[i]);
            var option = Instantiate(Debuff[selectedImg[i]], backgroundImgs[i].transform);
        }
    }

    private string DebuffText(int number)
    {
        string str;
        switch(number)
        {
            case 0:
                str = "보스 공격력 증가 + 5";
                break;
            case 1:
                str = "보스 속도 증가 + 5%";
                break;
            case 2:
                str = "완벽 가드 시간 감소 \n - 0.02초";
                break;
            case 3:
                str = "스테미나 회복속도 감소 \n - 6%";
                break;
            case 4:
                str = "보스 2페이즈 시작";
                break;
            default:
                str = "";
                Debug.Log("빈칸");
                break;
        }   
        return str;
    }

    public void PointDownDebuffOption(BaseEventData data)
    {
        var selected = data.selectedObject;
        int number = Int32.Parse(selected.GetComponent<Image>().name);

        switch(selectedImg[number])
        {
            case 0:
                debuffBossDamageUp += 1;
                break;
            case 1:
                debuffBossSpeedUp += 1;
                break;
            case 2:
                debuffPerfectDefenseTimeDown += 1;
                break;
            case 3:
                debuffStaminaRegenDown += 1;
                break;
            case 4:
                debuffBossStartPhase += 1;
                break;
            default:
                return;
        }
        debuffCount += 1;
        Save();
        SceneManager.LoadScene("Battle_1");
    }

    public void CancelButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void HideGuide()
    {
        GuideUI.SetActive(false);
        Time.timeScale = 1f;
        VolumeOn();
    }

    private void VolumeOff()
    {
        foreach(var source in audiosources)
        {
            if(source.outputAudioMixerGroup != BGM)
                source.mute = true;
        }
    }

    private void VolumeOn()
    {
        foreach(var source in audiosources)
        {
            if(source.outputAudioMixerGroup != BGM)
                source.mute = false;
        }
    }

    //Debuff property
    public static float DebuffBossDamageUp
    {
        get
        {
            return gameManager.debuffBossDamageUp * 5f;
        }
        set
        {
            gameManager.debuffBossDamageUp = (int)value;
        }
    }

    public static float DebuffBossSpeedUp
    {
        get
        {
            return gameManager.debuffBossSpeedUp * 0.05f;
        }
        set
        {
            gameManager.debuffBossSpeedUp = (int)value;
        }
    }

    public static float DebuffPerfectDefenseTimeDown
    {
        get
        {
            return gameManager.debuffPerfectDefenseTimeDown * 0.02f;
        }
        set
        {
            gameManager.debuffPerfectDefenseTimeDown = (int)value;
        }
    }

    public static float DebuffStaminaRegenDown
    {
        get
        {
            return gameManager.debuffStaminaRegenDown * 2f;
        }
        set
        {
            gameManager.debuffStaminaRegenDown = (int)value;
        }
    }

    public static int DebuffBossStartPhase
    {
        get
        {
            return gameManager.debuffBossStartPhase;
        }
        set
        {
            gameManager.debuffBossStartPhase = value;
        }
    }
}
