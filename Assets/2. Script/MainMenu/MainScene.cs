using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public class MainScene : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                Application.Quit();
        }
    }
}
