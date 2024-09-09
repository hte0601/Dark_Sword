using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public enum BoardUI
    {
        Title,
        Info,
        GameSetting,
        SpeedMode
    }

    public class BoardUIManager : MonoBehaviour
    {
        public static BoardUIManager instance;

        [SerializeField] private GameObject titleBoard;
        [SerializeField] private GameObject infoBoard;
        [SerializeField] private GameObject gameSettingBoard;
        [SerializeField] private GameObject speedModeBoard;

        private Dictionary<BoardUI, GameObject> boardUIDict = new();
        private BoardUI currentBoard;

        private void Awake()
        {
            instance = this;

            boardUIDict.Add(BoardUI.Title, titleBoard);
            boardUIDict.Add(BoardUI.Info, infoBoard);
            boardUIDict.Add(BoardUI.GameSetting, gameSettingBoard);
            boardUIDict.Add(BoardUI.SpeedMode, speedModeBoard);

            currentBoard = BoardUI.Title;

            foreach (var item in boardUIDict)
            {
                if (item.Key == BoardUI.Title)
                    item.Value.SetActive(true);
                else
                    item.Value.SetActive(false);
            }
        }

        public void OpenBoardUI(BoardUI board, bool isToggle = false)
        {
            if (currentBoard == BoardUI.Title)
            {
                boardUIDict[BoardUI.Title].SetActive(false);
                boardUIDict[board].SetActive(true);

                currentBoard = board;
            }
            else if (currentBoard == board)
            {
                if (!isToggle) return;

                boardUIDict[board].SetActive(false);
                boardUIDict[BoardUI.Title].SetActive(true);

                currentBoard = BoardUI.Title;
            }
            else
            {
                boardUIDict[currentBoard].SetActive(false);
                boardUIDict[board].SetActive(true);

                currentBoard = board;
            }
        }

        public void CloseBoardUI(GameObject boardObj)
        {
            boardObj.SetActive(false);

            if (boardUIDict[currentBoard] == boardObj)
            {
                boardUIDict[BoardUI.Title].SetActive(true);
                currentBoard = BoardUI.Title;
            }
        }

        // public void CloseBoardUI(BoardUI board)
        // {
        //     if (currentBoard == board)
        //     {
        //         currentBoard = BoardUI.None;
        //     }

        //     boardUIDict[board].SetActive(false);
        // }
    }
}
