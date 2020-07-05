using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReisenGameManager : MonoBehaviour
{
    public GameObject player;
    public ReisenGameProgress gameProgress;

    public static ReisenGameManager instance;

    public void Awake()
    {
        if (ReisenGameManager.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);
    }

    public void SaveGame(string saveName, ReisenSavePoint savePoint)
    {
        gameProgress.savePoint = savePoint;

        SaveManager.SaveGame(saveName, gameProgress);
    }

    public void LoadGame(string saveName)
    {
        gameProgress = SaveManager.LoadGame(saveName);
        SceneManager.LoadScene(gameProgress.savePoint.sceneName);

        gameProgress.savePoint.SpawnPlayer(player);
    }
}
