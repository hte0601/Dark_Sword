using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoardUI
{
    None,
    Info,
    GameSetting,
    Battle
}

public class MainSceneBoardsUI : MonoBehaviour
{
    public static MainSceneBoardsUI instance;
    [SerializeField] private GameObject infoBoard;
    [SerializeField] private GameObject gameSettingBoard;
    [SerializeField] private GameObject battleBoard;
    private Dictionary<BoardUI, GameObject> boardUIDict = new();
    private BoardUI currentBoard = BoardUI.None;

    private void Awake()
    {
        instance = this;

        boardUIDict.Add(BoardUI.Info, infoBoard);
        boardUIDict.Add(BoardUI.GameSetting, gameSettingBoard);
        boardUIDict.Add(BoardUI.Battle, battleBoard);
    }

    public void OpenBoardUI(BoardUI board, bool isToggle = false)
    {
        if (currentBoard == BoardUI.None)
        {
            currentBoard = board;
            boardUIDict[board].SetActive(true);
        }
        else if (currentBoard == board)
        {
            if (!isToggle) return;

            currentBoard = BoardUI.None;
            boardUIDict[board].SetActive(false);
        }
        else
        {
            boardUIDict[currentBoard].SetActive(false);
            currentBoard = board;
            boardUIDict[board].SetActive(true);
        }
    }

    public void CloseBoardUI(BoardUI board)
    {
        if (currentBoard == board)
        {
            currentBoard = BoardUI.None;
        }

        boardUIDict[board].SetActive(false);
    }

    public void CloseBoardUI(GameObject board)
    {
        if (boardUIDict[currentBoard] == board)
        {
            currentBoard = BoardUI.None;
        }

        board.SetActive(false);
    }
}
