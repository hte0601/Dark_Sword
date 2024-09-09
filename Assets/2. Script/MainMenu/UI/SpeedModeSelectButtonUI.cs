using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMenu
{
    public class SpeedModeSelectButtonUI : MonoBehaviour
    {
        [SerializeField] SpeedMode.GameMode.Mode gameMode;

        private SpeedModeBoardUI speedModeBoardUI;

        private void Awake()
        {
            if (!transform.parent.TryGetComponent(out speedModeBoardUI))
            {
#if UNITY_EDITOR
                Debug.Log("SpeedModeBoardUI의 스크립트 컴포넌트를 찾을 수 없음");
#endif
            }
        }

        public void OnModeSelectButtonClick()
        {
            speedModeBoardUI.LoadSpeedScene(gameMode);
        }
    }
}
