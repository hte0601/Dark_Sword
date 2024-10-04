using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class QuickButtonsUI : MonoBehaviour
    {
        [SerializeField] private GameObject soundButton;

        private BoardUIManager boardUIManager;
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
            boardUIManager = BoardUIManager.instance;
            volumeManager = GameSystem.VolumeManager.instance;

            volumeManager.OnIsMutedChanged += ChangeSoundButtonSprite;
            ChangeSoundButtonSprite(volumeManager.IsMuted);
        }

        private void OnDestroy()
        {
            volumeManager.OnIsMutedChanged -= ChangeSoundButtonSprite;
        }


        private void ChangeSoundButtonSprite(bool isMuted)
        {
            soundButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }

        public void OnSoundButtonClick()
        {
            volumeManager.IsMuted = !volumeManager.IsMuted;
        }

        public void OnInfoButtonClick()
        {
            boardUIManager.OpenBoardUI(BoardUI.Info, true);
        }

        public void OnGameSettingButtonClick()
        {
            boardUIManager.OpenBoardUI(BoardUI.GameSetting, true);
        }
    }
}
