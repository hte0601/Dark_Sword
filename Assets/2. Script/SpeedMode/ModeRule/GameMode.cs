using System;
using UnityEngine;

namespace SpeedMode
{
    public class GameMode : MonoBehaviour
    {
        public enum Mode
        {
            Test,

            Normal,
            Hard,
            Infinite,

            // Challenge
            C_OneLife,
            C_Faster,
        }


        public static GameMode instance;
        public static Mode? currentMode = null;

        public ModeRule modeRule;


        private void Awake()
        {
            instance = this;

            SetModeRule();
        }

        private void SetModeRule()
        {
            if (currentMode == Mode.Test)
            {
                modeRule = new TestModeRule();
            }
            else if (currentMode == Mode.Normal)
            {
                modeRule = new NormalModeRule();
            }
            else if (currentMode == Mode.Hard)
            {
                modeRule = new HardModeRule();
            }
#if UNITY_EDITOR
            else
            {
                modeRule = null;
                Debug.Log("modeRule 초기화 오류");
            }
#endif
        }
    }
}
