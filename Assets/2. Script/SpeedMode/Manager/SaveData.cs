using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeedMode
{
    public class SaveData : MonoBehaviour
    {
        public static SaveData instance;
        public PlayData playData;

        private void Awake()
        {
            instance = this;
            playData = SaveManager.Load<PlayData>(PlayData.KEY);
        }
    }
}
