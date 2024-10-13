using System;
using UnityEngine;

namespace SpeedMode
{
    public class GameMode : MonoBehaviour
    {
        public enum Mode
        {
            Test = -1,

            Normal = 0,
            Hard = 1,

            Infinite = 10,

            // Challenge
            C_OneLife = 20,
            C_Faster = 21,
        }


        public static GameMode instance;
        public static Mode? currentMode = null;

        public ModeData modeData;


        private void Awake()
        {
            instance = this;

            SetModeData();
        }

        private void SetModeData()
        {
            if (currentMode == Mode.Test)
            {
                modeData = new TestModeData();
            }
            else if (currentMode == Mode.Normal)
            {
                modeData = new NormalModeData();
            }
            else if (currentMode == Mode.Hard)
            {
                modeData = new HardModeData();
            }
            else if (currentMode == Mode.Infinite)
            {
                modeData = new InfiniteModeData();
            }
            else if (currentMode == Mode.C_OneLife)
            {
                modeData = new C_OneLifeModeData();
            }
            else if (currentMode == Mode.C_Faster)
            {
                modeData = new C_FasterModeData();
            }
#if UNITY_EDITOR
            else if (currentMode == null)
            {
                Debug.LogError($"modeData 초기화 오류, currentMode: null");
                modeData = null;
            }
            else
            {
                Debug.LogError($"modeData 초기화 오류, currentMode: {currentMode}");
                modeData = null;
            }
#endif
        }
    }
}
