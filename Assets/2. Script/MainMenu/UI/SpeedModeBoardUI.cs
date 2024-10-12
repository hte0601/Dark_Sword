using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpeedMode;

namespace MainMenu
{
    public class SpeedModeBoardUI : MonoBehaviour
    {
        public void LoadSpeedScene(GameMode.Mode gameMode)
        {
            GameMode.currentMode = gameMode;
            SceneManager.LoadScene("Speed");
        }

        public void OnExitButtonClick()
        {
            BoardUIManager.instance.CloseBoardUI(gameObject);
        }
    }
}
