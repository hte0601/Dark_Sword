using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleModeBoardUI : MonoBehaviour
{
    public void OnNewBattleButtonPointerDown()
    {
        BattleGameManager.DebuffReset();
        SceneManager.LoadScene("Battle_1");
    }

    public void OnContinueBattleButtonPointerDown()
    {
        SceneManager.LoadScene("Battle_1");
    }

    public void OnExitButtonPointerDown()
    {
        MainSceneBoardsUI.instance.SetBoardUIActive(BoardUI.Battle, true);
    }
}