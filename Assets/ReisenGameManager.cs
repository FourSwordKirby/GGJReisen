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

    private void Update()
    {
        if(!RpgGameManager.instance.Paused)
        {
            if(Controls.pauseInputDown())
            {
                StartPauseProcess();
            }
        }
    }

    public void StartPauseProcess()
    {
        StartLoadProcess();
    }

    public void StartLoadProcess()
    {
        SaveUI.instance.Show(SavePanelMode.Loading);

        RpgGameManager.instance.PauseGameplay();
    }

    public void StartSaveProcess(ReisenSavePoint savePoint)
    {
        SaveUI.instance.currentSavePoint = savePoint;
        SaveUI.instance.Show(SavePanelMode.Saving);

        RpgGameManager.instance.PauseGameplay();
    }


    public void EndSaveProcess()
    {
        SaveUI.instance.Hide();

        RpgGameManager.instance.ResumeGameplay();
    }

    public void SaveGame(string saveName, ReisenSavePoint savePoint)
    {
        gameProgress.savePoint = savePoint;

        SaveManager.SaveGame(saveName, gameProgress);
    }

    public void LoadGame(string saveName)
    {
        gameProgress = SaveManager.FetchGameProgress(saveName);
        SceneManager.LoadScene(gameProgress.savePoint.sceneName);

        gameProgress.savePoint.SpawnPlayer(player);
    }
}
