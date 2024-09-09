using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public class GameStartButtonUI : MonoBehaviour
    {
        private BoardUIManager boardUIManager;

        private void Start()
        {
            boardUIManager = BoardUIManager.instance;
        }


        public void OnGameStartButtonClick()
        {
            boardUIManager.OpenBoardUI(BoardUI.SpeedMode, false);
        }
    }
}
