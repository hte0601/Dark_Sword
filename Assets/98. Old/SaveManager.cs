using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldGameSystem
{
    public static class SaveManager
    {
        private static Dictionary<Type, string> keyDict = new()
        {
            {typeof(GameSystem.VolumeSetting), "VolumeSetting"},
            {typeof(SpeedMode.PlayData), "SpeedModePlayData"},
            {typeof(SpeedMode.UpgradeData), "SpeedModeUpgradeData"}
        };

        public static void Save<T>(object obj) where T : ISaveData
        {
            string key = keyDict[typeof(T)];

            string json = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static T Load<T>() where T : ISaveData, new()
        {
            string key = keyDict[typeof(T)];

            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<T>(json);
            }
            else
            {
                return new T();
            }
        }
    }
}
