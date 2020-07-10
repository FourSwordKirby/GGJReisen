using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
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