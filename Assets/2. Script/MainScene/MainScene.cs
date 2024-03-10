﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        QuitGame();
    }

    void QuitGame()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
}
