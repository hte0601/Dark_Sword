using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif

#if UNITY_EDITOR
        Application.targetFrameRate = -1;
#endif
    }

    void Update()
    {
        GameQuit();
    }

    void GameQuit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
                Application.Quit();
        }
    }
}
