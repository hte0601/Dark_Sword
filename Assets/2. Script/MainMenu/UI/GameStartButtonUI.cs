using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public class GameStartButtonUI : MonoBehaviour
    {
        public void OnGameStartButtonClick()
        {
            BoardUIManager.instance.OpenBoardUI(BoardUI.SpeedMode, false);
        }
    }
}
