using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReisenSavePointInteractable : MonoBehaviour
{
    public Transform spawnTransform;
    public ReisenSavePoint savePoint;
    
    public void Awake()
    {
        savePoint.sceneName = SceneManager.GetActiveScene().name;
        savePoint.spawnLocation = spawnTransform.position;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ReisenGameManager.instance.SaveGame("test", savePoint);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ReisenGameManager.instance.LoadGame("test");
    }
}

[Serializable]
public class ReisenSavePoint
{
    public string sceneName;
    public Vector3 spawnLocation;

    public void SpawnPlayer(GameObject player)
    {
        player.transform.position = spawnLocation;
    }
}