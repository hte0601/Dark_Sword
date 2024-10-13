using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem
{
    public static class SaveDataManager
    {
        private static readonly Dictionary<Type, string> saveKeyDict = new()
        {
            // ISystemSaveData
            {typeof(GameSystem.VolumeSetting), "VolumeSetting"},
            {typeof(GameSystem.CurrencyData), "CurrencyData"},
            
            // ISingleGameSaveData
            {typeof(SpeedMode.UpgradeData), "SpeedMode_UpgradeData"},
            
            // IMultipleGameSaveData
            {typeof(SpeedMode.ModeStatisticData), "SpeedMode_ModeStatisticData"}
        };

        // 씬 전환 시에도 유지
        private static readonly Dictionary<string, ISaveData> loadedSystemData = new();

        // 씬 전환 시 해제
        private static readonly Dictionary<string, ISaveData> loadedGameData = new();


        static SaveDataManager()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            UnloadGameData();
        }

        private static void UnloadGameData()
        {
            loadedGameData.Clear();
        }


        public static T LoadData<T>() where T : ISingleSaveData, new()
        {
            string key = saveKeyDict[typeof(T)];

            // 이미 로드된 데이터인 경우
            if (loadedGameData.ContainsKey(key))
            {
                return (T)loadedGameData[key];
            }
            else if (loadedSystemData.ContainsKey(key))
            {
                return (T)loadedSystemData[key];
            }

            // 아직 로드되지 않은 데이터인 경우
            T data = LoadPlayerPrefs<T>(key);

            // 타입에 따라 딕셔너리에 저장
            if (data is ISingleGameSaveData)
            {
                loadedGameData.Add(key, data);
            }
            else if (data is ISystemSaveData)
            {
                loadedSystemData.Add(key, data);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError($"{typeof(T)} 클래스가 ISingleSaveData를 직접 구현함");
            }
#endif

            return data;
        }

        public static T LoadData<T>(int dataID) where T : IMultipleSaveData, new()
        {
            string key = saveKeyDict[typeof(T)] + "_" + dataID.ToString();

            // 이미 로드된 데이터인 경우
            if (loadedGameData.ContainsKey(key))
            {
                return (T)loadedGameData[key];
            }

            // 아직 로드되지 않은 데이터인 경우
            T data = LoadPlayerPrefs<T>(key);
            data.DataID = dataID;

            // 타입에 따라 딕셔너리에 저장
            if (data is IMultipleGameSaveData)
            {
                loadedGameData.Add(key, data);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError($"{typeof(T)} 클래스가 IMultipleSaveData 직접 구현함");
            }
#endif

            return data;
        }

        private static T LoadPlayerPrefs<T>(string key) where T : ISaveData, new()
        {
            if (PlayerPrefs.HasKey(key))
            {
                return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
            }
            else
            {
                return new T();
            }
        }


        public static void SaveData<T>(T data) where T : ISingleSaveData
        {
            string key = saveKeyDict[typeof(T)];

            PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        public static void SaveData<T>(T data, int dataID) where T : IMultipleSaveData
        {
            string key = saveKeyDict[typeof(T)] + "_" + dataID.ToString();

            PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }
    }
}
