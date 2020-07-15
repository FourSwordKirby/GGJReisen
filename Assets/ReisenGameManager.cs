using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReisenGameManager : MonoBehaviour
{
    public ReisenGameProgress gameProgress;
    public static ReisenGameManager instance;
    public int spawnLocation;


    public void Awake()
    {
        if (ReisenGameManager.instance == null)
        {
            instance = this;
        }
        else if (this != instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        InitSceneState();
    }

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if(!RpgGameManager.instance.Paused)
        {
            if(Controls.pauseInputDown())
            {
                StartGamePauseProcess();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
            InitSceneState();
    }

    public void StartNewGame()
    {
        Debug.Log("starting new game");
        throw new Exception("not yet implemented");
    }

    public void InitSceneState()
    {
        // bunch of bad game specific object goes here
        GameObject Npc = GameObject.Find("Keine");
        if (Npc != null)
            Npc.GetComponent<Npc>().InitNpcState(gameProgress.Keine);

        Npc = GameObject.Find("Kosuzu");
        if (Npc != null)
            Npc.GetComponent<Npc>().InitNpcState(gameProgress.Kosuzu);

        Npc = GameObject.Find("Nitori");
        if (Npc != null)
            Npc.GetComponent<Npc>().InitNpcState(gameProgress.Nitori);

        Npc = GameObject.Find("Akyu");
        if (Npc != null)
            Npc.GetComponent<Npc>().InitNpcState(gameProgress.Akyu);

        Npc = GameObject.Find("LunarReisen");
        if (Npc != null)
            Npc.GetComponent<Npc>().InitNpcState(gameProgress.LunarReisen);

        Npc = GameObject.Find("Miyoi");
        if (Npc != null)
            Npc.GetComponent<Npc>().InitNpcState(gameProgress.Miyoi);

        Npc = GameObject.Find("Kogasa");
        if (Npc != null)
            Npc.GetComponent<Npc>().InitNpcState(gameProgress.Kogasa);

    }

    //PARSE CONDITIONALS HERE
    //really hacky
    //Honestly this conditions satisfied logic being game specific is bad and can/should be adjusted qq
    internal bool ConditionsSatisfied(List<string> conditions)
    {
        bool finalBool = true;
        foreach(string condition in conditions)
        {
            string variable = condition.Split(new Char[] { '>', '=', '<' })[0];
            int value = int.Parse(condition.Split(new Char[] { '>', '=', '<' })[1]);
            Regex r = new Regex(@"\=|\>|\<");
            string comparator = r.Match(condition).Value;

            //Debug.Log(variable + value + comparator);
            //Debug.Log(variable + value + comparator);

            bool conditionalResult = false;

            // Item ownership checks
            if (variable == ReisenGameProgress.HasCough)
                conditionalResult = gameProgress.Player.CoughingMedicine == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasTextbook)
                conditionalResult = gameProgress.Player.TextBook == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasEncyclopedia)
                conditionalResult = gameProgress.Player.Encyclopedia == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasNewspaper)
                conditionalResult = gameProgress.Player.Newspaper == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasSmartphone)
                conditionalResult = gameProgress.Player.Smartphone == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasMagazine)
                conditionalResult = gameProgress.Player.Magazine == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasWrench)
                conditionalResult = gameProgress.Player.Wrench == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasScroll)
                conditionalResult = gameProgress.Player.Scroll == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasSchematic)
                conditionalResult = gameProgress.Player.Schematic == Assignment.Inventory;
            else if (variable == ReisenGameProgress.HasNovel)
                conditionalResult = gameProgress.Player.Novel == Assignment.Inventory;
            else if (variable == ReisenGameProgress.ElixirCount)
                conditionalResult = gameProgress.Player.Elixir1 == Assignment.Inventory || gameProgress.Player.Elixir2 == Assignment.Inventory;
            // character stage progress checks
            else if (comparator == "=")
            {
                if (variable == ReisenGameProgress.KeineStage)
                    conditionalResult = gameProgress.Keine.Stage == value;
                else if (variable == ReisenGameProgress.KosuzuStage)
                    conditionalResult = gameProgress.Kosuzu.Stage == value;
                else if (variable == ReisenGameProgress.NitoriStage)
                    conditionalResult = gameProgress.Nitori.Stage == value;
                else if (variable == ReisenGameProgress.AkyuStage)
                    conditionalResult = gameProgress.Akyu.Stage == value;
                else if (variable == ReisenGameProgress.LunarReisenStage)
                    conditionalResult = gameProgress.LunarReisen.Stage == value;
                else if (variable == ReisenGameProgress.MiyoiStage)
                    conditionalResult = gameProgress.Miyoi.Stage == value;
                else if (variable == ReisenGameProgress.KogasaStage)
                    conditionalResult = gameProgress.Kogasa.Stage == value;
                else
                    throw new Exception("broken conditional" + variable);
            }
            else if (comparator == ">")
            {
                if (variable == ReisenGameProgress.KeineStage)
                    conditionalResult = gameProgress.Keine.Stage > value;
                else if (variable == ReisenGameProgress.KosuzuStage)
                    conditionalResult = gameProgress.Kosuzu.Stage > value;
                else if (variable == ReisenGameProgress.NitoriStage)
                    conditionalResult = gameProgress.Nitori.Stage > value;
                else if (variable == ReisenGameProgress.AkyuStage)
                    conditionalResult = gameProgress.Akyu.Stage > value;
                else if (variable == ReisenGameProgress.LunarReisenStage)
                    conditionalResult = gameProgress.LunarReisen.Stage > value;
                else if (variable == ReisenGameProgress.MiyoiStage)
                    conditionalResult = gameProgress.Miyoi.Stage > value;
                else if (variable == ReisenGameProgress.KogasaStage)
                    conditionalResult = gameProgress.Kogasa.Stage > value;
                else
                    throw new Exception("broken conditional" + variable);
            }
            else if (comparator == "<")
            {
                if (variable == ReisenGameProgress.KeineStage)
                    conditionalResult = gameProgress.Keine.Stage < value;
                else if (variable == ReisenGameProgress.KosuzuStage)
                    conditionalResult = gameProgress.Kosuzu.Stage < value;
                else if (variable == ReisenGameProgress.NitoriStage)
                    conditionalResult = gameProgress.Nitori.Stage < value;
                else if (variable == ReisenGameProgress.AkyuStage)
                    conditionalResult = gameProgress.Akyu.Stage < value;
                else if (variable == ReisenGameProgress.LunarReisenStage)
                    conditionalResult = gameProgress.LunarReisen.Stage < value;
                else if (variable == ReisenGameProgress.MiyoiStage)
                    conditionalResult = gameProgress.Miyoi.Stage < value;
                else if (variable == ReisenGameProgress.KogasaStage)
                    conditionalResult = gameProgress.Kogasa.Stage < value;
                else
                    throw new Exception("broken conditional" + variable);
            }

            finalBool &= conditionalResult;
        }
        return finalBool;
    }

    public void StartGamePauseProcess()
    {
        StartLoadProcess();
    }

    public void StartLoadProcess()
    {
        PauseUI.instance.pauseMenuUI.Open();

        //PauseUI.instance.saveUI.Show(SavePanelMode.Loading);

        RpgGameManager.instance.PauseGameplay();
    }

    public void StartSaveProcess(ReisenSavePoint savePoint)
    {
        PauseUI.instance.saveUI.currentSavePoint = savePoint;
        PauseUI.instance.saveUI.Show(SavePanelMode.Saving);

        RpgGameManager.instance.PauseGameplay();
    }

    public void SaveGame(string saveName, ReisenSavePoint savePoint)
    {
        gameProgress.savePoint = savePoint;

        SaveManager.SaveGame(saveName, gameProgress);
    }

    public virtual void LoadGame(string saveName)
    {
        StartCoroutine(LoadGameSequence(saveName));
    }

    IEnumerator LoadGameSequence(string saveName)
    {
        yield return TransitionManager.instance.screenFader.FadeOut();
        gameProgress = SaveManager.FetchGameProgress(saveName);
        SceneManager.LoadScene(gameProgress.savePoint.sceneName);

        spawnLocation = -1;

        gameProgress.savePoint.SpawnPlayer(RpgPlayer.instance.gameObject);

        yield return TransitionManager.instance.screenFader.FadeIn();

        RpgGameManager.instance.ResumeGameplay();
    }

    //Scene transition things to manage later
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(spawnLocation != -1)
        {
            GameObject player = RpgPlayer.instance.gameObject;
            player.transform.position = FindObjectOfType<ReisenSceneManager>().sceneEntrances[spawnLocation].spawnArea.position;
        }

        InitSceneState();
    }

    public void SceneExit(string targetSceneName, int sceneEntranceIndex)
    {
        StartCoroutine(SwitchSceneSequence(targetSceneName, sceneEntranceIndex));
    }

    IEnumerator SwitchSceneSequence(string sceneName, int sceneEntranceIndex)
    {
        GameObject player = RpgPlayer.instance.gameObject;

        Debug.Log("trying to load at this entrance" + sceneEntranceIndex);
        spawnLocation = sceneEntranceIndex;

        yield return TransitionManager.instance.screenFader.FadeOut();

        SceneManager.LoadScene(sceneName);

        yield return TransitionManager.instance.screenFader.FadeIn();

        yield return null;
        //AudioManager.instance.OnNextLevelUnlock();
        //AudioManager.instance.OnPhaseAnyLevelFadeOut();

        //List<string> dialogEntries = DialogEngine.CreateDialogComponents(closingBanter.text);
        //ShmupGameManager.instance.PauseGameplay();
        //ConversationController.instance.StartConversation(dialogEntries);
        //yield return null;

        //while (ShmupGameManager.instance.Paused)
        //    yield return null;

        //ChapterHud.instance.EndLevel();
        //while (!ChapterHud.instance.AnimationFinished())
        //{
        //    yield return null;
        //}
        //SceneManager.LoadScene(2);
    }
}