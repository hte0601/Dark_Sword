using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class QuickButtonsUI : MonoBehaviour
    {
        private GameSystem.VolumeSetting volumeSetting;

        [SerializeField] private GameObject soundButton;

        private Image soundButtonImage;
        private Sprite soundOnSprite;
        private Sprite soundOffSprite;

        private void Awake()
        {
            soundButtonImage = soundButton.GetComponent<Image>();
            soundOnSprite = Resources.Load<Sprite>("UI/SoundON");
            soundOffSprite = Resources.Load<Sprite>("UI/SoundOFF");

            volumeSetting = GameSystem.GameSetting.volume;

            volumeSetting.OnMuteStateChanged += ChangeSoundButtonSprite;
            ChangeSoundButtonSprite(volumeSetting.IsMuted);
        }

        private void OnDestroy()
        {
            volumeSetting.OnMuteStateChanged -= ChangeSoundButtonSprite;
        }

        private void ChangeSoundButtonSprite(bool isMuted)
        {
            soundButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }

        public void OnSoundButtonPointerDown()
        {
            volumeSetting.IsMuted = !volumeSetting.IsMuted;
            volumeSetting.Save();
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
