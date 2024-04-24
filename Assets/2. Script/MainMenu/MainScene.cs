using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public class MainScene : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif

#if UNITY_EDITOR
            Application.targetFrameRate = 75;
#endif
        }

        void Update()
        {
            QuitGame();
        }

        void QuitGame()
        {
            if (Input.GetKey(KeyCode.Escape))
                Application.Quit();
        }
    }
}
