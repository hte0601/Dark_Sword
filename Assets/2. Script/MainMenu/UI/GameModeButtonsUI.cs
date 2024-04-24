using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class GameModeButtonsUI : MonoBehaviour
    {
        public void OnSpeedModeButtonPointerDown()
        {
            SceneManager.LoadScene("Speed");
        }

        public void OnBattleModeButtonPointerDown()
        {
            BoardUIManager.instance.OpenBoardUI(BoardUI.Battle, true);
        }
    }
}
