using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            InitScreen();
        }

        private void InitScreen()
        {
            Debug.Log(Screen.currentResolution.refreshRateRatio);

            // Android
#if UNITY_ANDROID
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
#endif

            // Windows
#if UNITY_STANDALONE_WIN
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1;
            Screen.SetResolution(1440, 720, false);
#endif
        }
    }
}

