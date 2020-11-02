using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[Serializable]
public class ShardSaveData
{
    [SerializeField]
    public List<Shard> shardData;

    public ShardSaveData(List<Shard> shardData)
    {
        this.shardData = shardData;
    }
}

[Serializable]
public class EndingSaveData
{
    [SerializeField]
    public List<int> SeenEndings;

    public EndingSaveData(List<int> seenEndings)
    {
        this.SeenEndings = seenEndings;
    }
}

public class SaveManager : MonoBehaviour
{
    public static readonly List<string> saveNames = new List<string>() { "Waning Moon", "Full Moon", "Waxing Moon" };
    public static readonly string shardSaveDataName = "shardsave";
    public static readonly string endingSaveDataName = "endingsave";

    public static void SaveSeenShardData(ReisenGameProgress gameProgress)
    {
        List<Shard> totalSeenShardData;
        ShardSaveData totalData = FetchSeenShardData();

        if (totalData != null)
            totalSeenShardData = totalData.shardData;
        else
            totalSeenShardData = new List<Shard>();

        foreach (Shard s in gameProgress.Player.ShardsAcquired)
        {
            if(!totalSeenShardData.Any(x => x.Id == s.Id))
            {
                totalSeenShardData.Add(s);
            }
        }

        ShardSaveData data = new ShardSaveData(totalSeenShardData);

        // 2
        string jsonSavePath = string.Format("{0}/{1}.json", Application.persistentDataPath, shardSaveDataName);
        Debug.Log(jsonSavePath);
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonSavePath, jsonData);

        Debug.Log("Shard data Saved");
    }

    public static ShardSaveData FetchSeenShardData()
    {
        // 1
        string jsonSavePath = string.Format("{0}/{1}.json", Application.persistentDataPath, shardSaveDataName);
        if (File.Exists(jsonSavePath))
        {
            Debug.Log("Attempting to load at " + jsonSavePath);
            ShardSaveData shardSave = JsonUtility.FromJson<ShardSaveData>(File.ReadAllText(jsonSavePath));
            Debug.Log("Shard data fetched");
            return shardSave;
        }
        else
        {
            return null;
        }
    }

    public static bool CheckIfSaveDataPresent(string saveName)
    {
        string jsonSavePath = string.Format("{0}/{1}.json", Application.persistentDataPath, saveName);
        return File.Exists(jsonSavePath);
    }

    public static void SaveGame(string saveName, ReisenGameProgress gameProgress)
    {
        // 1
        GGJReisenSave save = new GGJReisenSave(saveName, gameProgress);

        // 2
        string jsonSavePath = string.Format("{0}/{1}.json", Application.persistentDataPath, saveName);
        Debug.Log(jsonSavePath);
        string jsonData = JsonUtility.ToJson(save, true);
        File.WriteAllText(jsonSavePath, jsonData);

        Debug.Log("Game Saved");
    }

    public static GGJReisenSave FetchSaveData(string saveName)
    {
        // 1
        string jsonSavePath = string.Format("{0}/{1}.json", Application.persistentDataPath, saveName);
        if (File.Exists(jsonSavePath))
        {
            GGJReisenSave save = JsonUtility.FromJson<GGJReisenSave>(File.ReadAllText(jsonSavePath));
            return save;
        }
        else
        {
            return null;
        }
    }

    public static ReisenGameProgress FetchGameProgress(string saveName)
    {
        // 1
        string jsonSavePath = string.Format("{0}/{1}.json", Application.persistentDataPath, saveName);
        if (File.Exists(jsonSavePath))
        {
            GGJReisenSave save = JsonUtility.FromJson<GGJReisenSave>(File.ReadAllText(jsonSavePath));
            Debug.Log("Game Loaded");
            return save.gameProgress;
        }
        else
        {
            throw new Exception("save not found");
        }
    }

    public static void DeleteAllData()
    {
        DeleteShardData();
        foreach(string name in saveNames)
        {
            DeleteSaveData(name);
        }
        DeleteEndingData();
    }

    public static void DeleteShardData()
    {
        string jsonSavePath = string.Format("{0}/{1}.json", Application.persistentDataPath, shardSaveDataName);
        File.Delete(jsonSavePath);
    }

    public static void DeleteSaveData(string saveName)
    {
        // 1
        string jsonSavePath = string.Format("{0}/{1}.json", Application.persistentDataPath, saveName);
        File.Delete(jsonSavePath);
    }

    public static void SaveEndingProgress(int endingNumber)
    {
        List<int> currentEndingProgress = GetAcquiredEndings();
        if(!currentEndingProgress.Contains(endingNumber))
        {
            currentEndingProgress.Add(endingNumber);
        }

        EndingSaveData data = new EndingSaveData(currentEndingProgress);
        // 2
        string jsonEndingPath = string.Format("{0}/{1}.json", Application.persistentDataPath, endingSaveDataName);
        Debug.Log(jsonEndingPath);
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonEndingPath, jsonData);

        Debug.Log("Ending Progress Saved");
    }

    public static void DeleteEndingData()
    {
        // 1
        string jsonEndingPath = string.Format("{0}/{1}.json", Application.persistentDataPath, endingSaveDataName);
        File.Delete(jsonEndingPath);
    }

    public static List<int> GetAcquiredEndings()
    {
        // 1
        string jsonEndingPath = string.Format("{0}/{1}.json", Application.persistentDataPath, endingSaveDataName);
        if (File.Exists(jsonEndingPath))
        {
            EndingSaveData save = JsonUtility.FromJson<EndingSaveData>(File.ReadAllText(jsonEndingPath));
            return save.SeenEndings;
        }
        return new List<int>();
    }
}

[Serializable]
public class GGJReisenSave
{
    public string saveName;
    public string saveTime;
    public ReisenGameProgress gameProgress;

    public GGJReisenSave(string saveName, ReisenGameProgress gameProgress)
    {
        this.saveName = saveName;
        this.saveTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        Debug.Log(saveTime);
        this.gameProgress = gameProgress;
    }
}