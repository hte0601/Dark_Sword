using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public static class GameSetting
    {
        public static VolumeSetting volume;

        // 정적 생성자
        static GameSetting()
        {
            volume = SaveManager.Load<VolumeSetting>();
        }
    }
}
