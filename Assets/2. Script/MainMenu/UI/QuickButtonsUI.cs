using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class QuickButtonsUI : MonoBehaviour
    {
        [SerializeField] private GameObject soundButton;
        private GameSystem.VolumeManager volumeManager;

        private Image soundButtonImage;
        private Sprite soundOnSprite;
        private Sprite soundOffSprite;

        private void Awake()
        {
            soundButtonImage = soundButton.GetComponent<Image>();
            soundOnSprite = Resources.Load<Sprite>("UI/SoundON");
            soundOffSprite = Resources.Load<Sprite>("UI/SoundOFF");
        }

        private void Start()
        {
            volumeManager = GameSystem.VolumeManager.instance;

            volumeManager.OnMuteStateChanged += ChangeSoundButtonSprite;
            ChangeSoundButtonSprite(volumeManager.IsMuted);
        }

        private void OnDestroy()
        {
            volumeManager.OnMuteStateChanged -= ChangeSoundButtonSprite;
        }

        private void ChangeSoundButtonSprite(bool isMuted)
        {
            soundButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }

        public void OnSoundButtonPointerDown()
        {
            volumeManager.IsMuted = !volumeManager.IsMuted;
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
