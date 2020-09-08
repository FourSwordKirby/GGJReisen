﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUtils : MonoBehaviour
{
    public static TitleScreenUtils instance;

    public StartMenuUI startMenu;

    public void Awake()
    {
        if (TitleScreenUtils.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);
    }

    public void Start()
    {
        AudioMaster.instance.PlayTrack("Title");
        TransitionManager.instance.screenFader.CutToColor(Color.black);
        StartCoroutine(TransitionManager.instance.screenFader.FadeIn(1.5f));
    }

    public void LoadGame(string saveName)
    {
        StartCoroutine(LoadGameSequence(saveName));
    }

    public ReisenGameProgress loadedGameProgress;
    IEnumerator LoadGameSequence(string saveName)
    {
        yield return TransitionManager.instance.screenFader.FadeOut();
        loadedGameProgress = SaveManager.FetchGameProgress(saveName);
        SceneManager.LoadScene(loadedGameProgress.savePoint.sceneName);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadedGameProgress.savePoint.SpawnPlayer(RpgPlayer.instance.gameObject);
        ReisenGameManager.instance.gameProgress = loadedGameProgress;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartNewGame()
    {
        StartCoroutine(NewGameSequence());
    }


    IEnumerator NewGameSequence()
    {
        yield return TransitionManager.instance.screenFader.FadeOut();
        SceneManager.LoadScene("Prologue");
    }

    public void DeleteData()
    {
        AudioMaster.instance.PlaySfx("DataDelete");
        SaveManager.DeleteAllData();
        startMenu.InitializeTitleMenu();
        startMenu.Open();
    }
}