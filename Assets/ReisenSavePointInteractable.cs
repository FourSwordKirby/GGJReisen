using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReisenSavePointInteractable : MonoBehaviour
{
    public Transform spawnTransform;
    public ReisenSavePoint savePoint;

    public const string saveFileName1 = "Waning Moon";
    public const string saveFileName2 = "Half Moon";
    public const string saveFileName3 = "Waxing Moon";

    public void Awake()
    {
        savePoint.sceneName = SceneManager.GetActiveScene().name;
        savePoint.spawnLocation = spawnTransform.position;
    }

    public void Update()
    {
        Debug.Log("clean this part up");

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ReisenGameManager.instance.SaveGame("test", savePoint);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ReisenGameManager.instance.LoadGame("test");
    }

    public void SaveGame(string saveFileName)
    {
        ReisenGameManager.instance.SaveGame(saveFileName, savePoint);
    }

    public void StartSaveProcess()
    {
        ReisenGameManager.instance.StartSaveProcess(savePoint);
    }

    public void TestEvent(string message)
    {
        Debug.Log(message);
    }
}

[Serializable]
public class ReisenSavePoint
{
    public string sceneName;
    public string locationName;
    public Vector3 spawnLocation;

    public void SpawnPlayer(GameObject player)
    {
        player.transform.position = spawnLocation;
    }
}