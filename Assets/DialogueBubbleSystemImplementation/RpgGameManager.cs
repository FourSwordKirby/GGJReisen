using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RpgGameManager : MonoBehaviour
{
    public delegate void AfterDialogueEvent();

    public static RpgGameManager instance;

    public void Awake()
    {
        if (RpgGameManager.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartConversation(TextAsset dialogue, Vector3 speakerPosition, Transform cameraPosition = null, AfterDialogueEvent afterEvent = null)
    {
        DialogueEngine.InitializeGenerators(SpeakingLine.CreateSpeakingLine, ExpressionLine.CreateInstructionLine, ChoiceLine.GenerateChoiceLine);
        List<ScriptLine> lines = DialogueEngine.CreateDialogueComponents(dialogue.text);
        Dialogue processedDialogue = new Dialogue(lines);
        StartCoroutine(PlayConversation(processedDialogue, cameraPosition, afterEvent));
    }

    internal IEnumerator PlayConversation(Dialogue dialogue, Transform cameraPosition = null, AfterDialogueEvent afterEvent = null)
    {
        // If a special camera position was provided, tell the camera man to use it.
        ConversationPause();

        //External library dependency
        CameraMan cameraMan = FindObjectOfType<CameraMan>();
        if (cameraPosition != null)
            cameraMan.StartCinematicMode(cameraPosition);


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterDialogueAnimator>().startTalking();

        DialogueBubbleUI.instance.init(dialogue);

        //Dictionary<string, DialogueAnimator> speakerDict = DialogueEngine.GetSpeakers(dialogue.text).ToDictionary(x => x, x => GameObject.Find(x).GetComponent<DialogueAnimator>());

        int lineTracker = 0;
        //string currentSpeaker = "";
        while (!dialogue.IsFinished)
        {
            ScriptLine line = dialogue.GetNextLine();
            line.PerformLine();

            while (!line.IsFinished())
            {
                /* disabled for now
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    lineTracker--;
                    StartCoroutine(DialogueBubbleUI.instance.animateLogs(lineTracker));
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    lineTracker++;
                    StartCoroutine(DialogueBubbleUI.instance.animateLogs(lineTracker));
                }
                */
                yield return null;
            }
            lineTracker++;
        }
        DialogueBubbleUI.instance.finishDialogue();
        player.GetComponent<CharacterDialogueAnimator>().stopTalking();

        while (!DialogueBubbleUI.instance.ready)
            yield return null;

        ConversationUnpause();

        /*
        cameraMan.EndCinematicMode();
        */

        afterEvent();
        yield return null;
    }

    //hacky gameplay pause implementation because hackathon
    private void ConversationPause()
    {
        //PauseGameplay();
    }
    private void ConversationUnpause()
    {
        //ResumeGameplay();
    }

    // Dependent on external player class
    bool gamePaused = false;
    public bool Paused { get { return gamePaused; } }
    public void PauseGameplay()
    {
        gamePaused = true;
        foreach (CharacterMovement entity in GameObject.FindObjectsOfType<CharacterMovement>())
            entity.enabled = false;
    }
    public void ResumeGameplay()
    {
        gamePaused = false;
        foreach (CharacterMovement entity in GameObject.FindObjectsOfType<CharacterMovement>())
            entity.enabled = true;
    }

}
