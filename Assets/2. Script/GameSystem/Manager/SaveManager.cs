using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public static class SaveManager
    {
        public static void Save(string key, object obj)
        {
            string json = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static T Load<T>(string key) where T : new()
        {
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
