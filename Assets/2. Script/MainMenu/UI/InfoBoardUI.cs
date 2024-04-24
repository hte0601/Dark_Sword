using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class InfoBoardUI : MonoBehaviour
    {
        [SerializeField] private Text versionText;

        private void Awake()
        {
            versionText.text = string.Format("버전 : {0}", Application.version);
        }

        public void OnExitButtonPointerDown()
        {
            BoardUIManager.instance.CloseBoardUI(gameObject);
        }
    }
}
