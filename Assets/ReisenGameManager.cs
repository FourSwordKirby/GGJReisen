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
    public static bool isNewGame;

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
    }

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CameraMan.instance.gameObject.transform.position = RpgPlayer.instance.transform.position;
        InitSceneState();
        if (isNewGame)
        {
            if (spawnLocation != -1)
            {
                StartCoroutine(SpawnPlayerAtEntrance());
                GameObject newGameObjects = GameObject.Find("NewGameTutorialObjects");
                for(int i = 0; i < newGameObjects.transform.childCount; i++)
                {
                    GameObject tutorialObject = newGameObjects.transform.GetChild(i).gameObject;
                    tutorialObject.SetActive(!tutorialObject.activeSelf);
                    AudioMaster.instance.PlayTrack("BambooForest");
                }
            }
            isNewGame = false;
        }
        else
            AudioMaster.instance.PlayVillageTrack();
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

    public void SyncNpcState()
    {
        // bunch of bad game specific object goes here
        GameObject Npc = GameObject.Find("Keine");
        if (Npc != null)
            Npc.GetComponent<Npc>().SyncNpcState(gameProgress.Keine);

        Npc = GameObject.Find("Kosuzu");
        if (Npc != null)
            Npc.GetComponent<Npc>().SyncNpcState(gameProgress.Kosuzu);

        Npc = GameObject.Find("Nitori");
        if (Npc != null)
            Npc.GetComponent<Npc>().SyncNpcState(gameProgress.Nitori);

        Npc = GameObject.Find("Akyu");
        if (Npc != null)
            Npc.GetComponent<Npc>().SyncNpcState(gameProgress.Akyu);

        Npc = GameObject.Find("LunarReisen");
        if (Npc != null)
            Npc.GetComponent<Npc>().SyncNpcState(gameProgress.LunarReisen);

        Npc = GameObject.Find("Miyoi");
        if (Npc != null)
            Npc.GetComponent<Npc>().SyncNpcState(gameProgress.Miyoi);

        Npc = GameObject.Find("Kogasa");
        if (Npc != null)
            Npc.GetComponent<Npc>().SyncNpcState(gameProgress.Kogasa);


        Npc = GameObject.Find("YinYangOrb");
        if (Npc != null)
            Npc.GetComponent<Npc>().SyncNpcState(gameProgress.Orb);
    }

    private void InitSceneState()
    {
        //Destroy items we already have
        ItemPickup itemPickup = GameObject.FindObjectOfType<ItemPickup>();
        if (itemPickup != null)
        {
            if (itemPickup.itemType == ReisenPickupItemType.Newspaper && gameProgress.Player.Newspaper != Assignment.NotAcquired)
            {
                Destroy(itemPickup.gameObject);
            }
            if (itemPickup.itemType == ReisenPickupItemType.Wrench && gameProgress.Player.Wrench != Assignment.NotAcquired)
            {
                Destroy(itemPickup.gameObject);
            }
        }

        //Set camera bounds properly
        if (FindObjectOfType<ReisenSceneManager>() != null)
            CameraMan.instance.CameraBounds = FindObjectOfType<ReisenSceneManager>().CameraBounds;

        //Sanity check
        if (gameProgress.Player.CoughMedicine == Assignment.NotAcquired)
        {
            Debug.Log("invalid state");
            gameProgress.Player.CoughMedicine = Assignment.Inventory;
        }

        SyncNpcState();
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
                conditionalResult = gameProgress.Player.CoughMedicine == Assignment.Inventory;
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
                else if (variable == ReisenGameProgress.ShardCount)
                    conditionalResult = gameProgress.Player.ShardsAcquired.Count == value;
                else
                    throw new Exception("broken conditional " + variable);
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
                else if (variable == ReisenGameProgress.ShardCount)
                    conditionalResult = gameProgress.Player.ShardsAcquired.Count > value;
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
                else if (variable == ReisenGameProgress.ShardCount)
                    conditionalResult = gameProgress.Player.ShardsAcquired.Count < value;
                else
                    throw new Exception("broken conditional" + variable);
            }

            finalBool &= conditionalResult;
        }
        return finalBool;
    }

    public void StartGamePauseProcess()
    {
        PauseUI.instance.pauseMenuUI.Open();

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
        SaveManager.SaveSeenShardData(gameProgress);
    }

    public virtual void LoadGame(string saveName)
    {
        spawnLocation = -1;
        StartCoroutine(LoadGameSequence(saveName));
    }

    IEnumerator LoadGameSequence(string saveName)
    {
        yield return TransitionManager.instance.screenFader.FadeOut();
        gameProgress = SaveManager.FetchGameProgress(saveName);
        SceneManager.LoadScene(gameProgress.savePoint.sceneName);

        spawnLocation = -1;

        gameProgress.savePoint.SpawnPlayer(RpgPlayer.instance.gameObject);

        CameraMan.instance.smoothMovement = false;
        yield return TransitionManager.instance.screenFader.FadeIn();
        CameraMan.instance.smoothMovement = true;

        RpgGameManager.instance.ResumeGameplay();
    }

    //Scene transition things to manage later
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitSceneState();
        if (spawnLocation != -1)
        {
            GameObject player = RpgPlayer.instance.gameObject;
            player.GetComponent<CharacterMovement>().forcedMove = true;
            StartCoroutine(SpawnPlayerAtEntrance());
        }
    }

    IEnumerator SpawnPlayerAtEntrance()
    {
        CameraMan.instance.CameraBounds = FindObjectOfType<ReisenSceneManager>().CameraBounds;
        CameraMan.instance.smoothMovement = false;

        GameObject player = RpgPlayer.instance.gameObject;

        SceneSwitchTrigger entrance = FindObjectOfType<ReisenSceneManager>().sceneEntrances[spawnLocation];
        entrance.triggerActive = false;
        Vector3 spawnPosition = entrance.spawnArea.position;
        Vector3 spawnDestination = entrance.spawnFinalPosition.position;
        player.transform.position = spawnPosition;

        yield return player.GetComponent<CharacterMovement>().moveCharacter(spawnDestination, spawnPosition-spawnDestination, 3.5f, 5.0f);
        CameraMan.instance.smoothMovement = true;
        entrance.triggerActive = true;
    }


    public void SceneExit(string targetSceneName, int sceneEntranceIndex, SceneSwitchTrigger exit)
    {
        StartCoroutine(SwitchSceneSequence(targetSceneName, sceneEntranceIndex, exit));
    }

    IEnumerator SwitchSceneSequence(string sceneName, int sceneEntranceIndex, SceneSwitchTrigger exit)
    {
        GameObject player = RpgPlayer.instance.gameObject;
        Vector3 spawnArea = exit.spawnArea.position;

        Debug.Log("trying to load at this entrance" + sceneEntranceIndex);
        spawnLocation = sceneEntranceIndex;

        player.GetComponent<CharacterMovement>().externalMoveCharacter(spawnArea, Vector3.zero, 1.5f, 5.0f);
        yield return TransitionManager.instance.screenFader.FadeOut();

        SceneManager.LoadScene(sceneName);

        yield return null;
    }


    //Wheeee more hacking
    public GameObject statusTextPrefab;
    public void ShowItemTransaction(List<string> transactions)
    {
        StartCoroutine(TransactionNotificationSequence(transactions));
    }

    public IEnumerator TransactionNotificationSequence(List<string> transactions)
    {
        foreach(string transaction in transactions)
        {
            StatusText statusText = Instantiate(statusTextPrefab).GetComponent<StatusText>();
            statusText.transform.position = RpgPlayer.instance.transform.position + Vector3.up;
            statusText.text.text = transaction;
            AudioMaster.instance.PlayItemGetSfx();
            yield return new WaitForSeconds(2.5f);
        }
    }

    internal void ReturnToTitle()
    {
        StartCoroutine(ReturnToTitleSequence());
    }

    IEnumerator ReturnToTitleSequence()
    {
        yield return TransitionManager.instance.screenFader.FadeOut();

        RpgGameManager.instance.ResumeGameplay();

        SceneManager.LoadScene("TitleScreen");

        Destroy(this.gameObject);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    internal void StartEnding()
    {
        StartCoroutine(EndingSequence());
        throw new Exception("Ending not yet implemented");
    }

    IEnumerator EndingSequence()
    {
        yield return TransitionManager.instance.screenFader.FadeOut();

        RpgGameManager.instance.ResumeGameplay();

        SceneManager.LoadScene("Ending1");

        Destroy(this.gameObject);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    internal void QuitGame()
    {
        StartCoroutine(QuitGameSequence());
    }
    IEnumerator QuitGameSequence()
    {
        yield return TransitionManager.instance.screenFader.FadeOut();
        Application.Quit();
    }
}