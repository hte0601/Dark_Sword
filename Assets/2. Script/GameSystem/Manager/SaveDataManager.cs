using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveDataManager
{
    private static readonly Dictionary<Type, string> saveKeyDict = new()
    {
        // ISystemSaveData
        {typeof(GameSystem.VolumeSetting), "VolumeSetting"},
        
        // IGameSaveData
        {typeof(SpeedMode.PlayData), "SpeedModePlayData"},
        {typeof(SpeedMode.UpgradeData), "SpeedModeUpgradeData"}
    };

    private static readonly Dictionary<Type, ISaveData> loadedSystemData = new();  // 씬 전환 시에도 유지
    private static readonly Dictionary<Type, ISaveData> loadedGameData = new();  // 씬 전환 시 해제


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


    public static T LoadData<T>() where T : ISaveData, new()
    {
        Type dataType = typeof(T);

        // 이미 로드된 데이터인 경우
        if (loadedGameData.ContainsKey(dataType))
            return (T)loadedGameData[dataType];

        if (loadedSystemData.ContainsKey(dataType))
            return (T)loadedSystemData[dataType];

        // 아직 로드되지 않은 데이터인 경우
        T data;
        string key = saveKeyDict[dataType];

        if (PlayerPrefs.HasKey(key))
            data = JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        else
            data = new T();

        // 타입에 따라 딕셔너리에 저장
        if (typeof(IGameSaveData).IsAssignableFrom(dataType))
        {
            loadedGameData.Add(dataType, data);
        }
        else if (typeof(ISystemSaveData).IsAssignableFrom(dataType))
        {
            loadedSystemData.Add(dataType, data);
        }
#if UNITY_EDITOR
        else
        {
            Debug.Log("LoadData 타입 오류");
        }
#endif

        return data;
    }

    public static void SaveData<T>(T data) where T : ISaveData
    {
        Type dataType = typeof(T);
        string key = saveKeyDict[dataType];

        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }
}
