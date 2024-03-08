using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeButtonsUI : MonoBehaviour
{
    public void OnSpeedModeButtonPointerDown() 
    {
        SceneManager.LoadScene("Speed");
    }

    public void OnBattleModeButtonPointerDown() 
    {
        MainSceneBoardsUI.instance.SetBoardUIActive(BoardUI.Battle, true);
    }
}
