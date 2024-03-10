using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBoardUI : MonoBehaviour
{
    [SerializeField] private Text versionText;

    private void Awake()
    {
        versionText.text = string.Format("버전 : {0}", Application.version);
    }

    public void OnExitButtonPointerDown()
    {
        MainSceneBoardsUI.instance.CloseBoardUI(gameObject);
    }
}
