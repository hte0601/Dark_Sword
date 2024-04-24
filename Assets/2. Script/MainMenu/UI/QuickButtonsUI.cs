using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

using GameSystem;

namespace MainMenu
{
    public class QuickButtonsUI : MonoBehaviour
    {
        [SerializeField] private GameObject soundButton;

        private AudioMixer mainMixer;
        private Image soundButtonImage;
        private Sprite soundOnSprite;
        private Sprite soundOffSprite;

        private void Awake()
        {
            soundButtonImage = soundButton.GetComponent<Image>();
            soundOnSprite = Resources.Load<Sprite>("UI/SoundON");
            soundOffSprite = Resources.Load<Sprite>("UI/SoundOFF");

            GameSetting.volume.OnIsMutedValueChanged += ChangeSoundButtonSprite;
            ChangeSoundButtonSprite(GameSetting.volume.IsMuted);
        }

        private void Start()
        {
            mainMixer = VolumeManager.instance.mainMixer;
        }

        private void OnDestroy()
        {
            GameSetting.volume.OnIsMutedValueChanged -= ChangeSoundButtonSprite;
        }

        private void ChangeSoundButtonSprite(bool isMuted)
        {
            soundButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }

        public void OnSoundButtonPointerDown()
        {
            if (GameSetting.volume.IsMuted)
            {
                // 음소거 해제 시
                if (GameSetting.volume.MasterVolume == 0.0001f)
                {
                    GameSetting.volume.MasterVolume = 0.2f;
                }

                mainMixer.SetFloat("MasterMix", GameSetting.volume.MasterMix);
                GameSetting.volume.IsMuted = false;
            }
            else
            {
                // 음소거 시
                mainMixer.SetFloat("MasterMix", -80);
                GameSetting.volume.IsMuted = true;
            }

            GameSetting.volume.Save();
        }

        public void OnInfoButtonPointerDown()
        {
            BoardUIManager.instance.OpenBoardUI(BoardUI.Info, true);
        }

        public void OnGameSettingButtonPointerDown()
        {
            BoardUIManager.instance.OpenBoardUI(BoardUI.GameSetting, true);
        }
    }
}
