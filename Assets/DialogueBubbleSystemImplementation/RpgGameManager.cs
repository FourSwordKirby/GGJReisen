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

    public void StartConversation(TextAsset dialogue, Vector3 focusPosition, Transform playerPosition = null, Transform cameraPosition = null, AfterDialogueEvent afterEvent = null, List<DialogueInstruction> AvailableInstructions = null)
    {
        DialogueEngine.InitializeGenerators(SpeakingLine.CreateSpeakingLine, ExpressionLine.CreateInstructionLine, ChoiceLine.GenerateChoiceLine, InstructionLine.GenerateInstructionline, StallLine.GenerateStallLine, ReisenGameManager.instance.ConditionsSatisfied);
        List<ScriptLine> lines = DialogueEngine.CreateDialogueComponents(dialogue.text, AvailableInstructions);
        HashSet<string> speakers = new HashSet<string>(lines.Select(x => x.speaker).Distinct());
        Dialogue processedDialogue = new Dialogue(lines, speakers);
        StartCoroutine(PlayConversation(processedDialogue, focusPosition, playerPosition, cameraPosition, afterEvent));
    }

    internal IEnumerator PlayConversation(Dialogue dialogue, Vector3 focusPosition, Transform playerPosition = null, Transform cameraPosition = null, AfterDialogueEvent afterEvent = null)
    {
        // If a special camera position was provided, tell the camera man to use it.
        ConversationPause();

        //External library dependency
        CameraMan cameraMan = FindObjectOfType<CameraMan>();

        // Start Cinematic Mode right away
        if (cameraPosition != null)
        {
            cameraMan.StartCinematicMode(cameraPosition);
            // Don't wait for Camera yet. Let forced player movement start working.
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (playerPosition != null)
        {
            // Locate the other speaker
            GameObject otherSpeaker = null;
            foreach (string speakerName in dialogue.speakers)
            {
                GameObject speakerObject = GameObject.Find(speakerName);
                if (speakerObject != player)
                {
                    otherSpeaker = speakerObject;
                }
            }

            SetNpcCollisionBoxes(otherSpeaker, false); // Disable NPC collision boxes while player is moving.
            yield return player.GetComponent<CharacterMovement>().moveCharacter(playerPosition.position, focusPosition-playerPosition.position, 6f);
            SetNpcCollisionBoxes(otherSpeaker, true);

            // If no cinematic camera position was given, and there is another "speaker" (probably an NPC),
            // put the camera into a magic "dialogue angle"
            if (cameraPosition == null && otherSpeaker != null)
            {
                Vector3 cameraOffset = new Vector3(0f, 2f, -4.5f); // Magical camera offset vector for 4:3 size.
                Vector3 midpointBetweenSpeakers = (otherSpeaker.transform.position + player.transform.position) * .5f;
                cameraMan.StartCinematicMode(midpointBetweenSpeakers + cameraOffset, Quaternion.identity);
            }
        }

        // Wait for Camera to be in place before we start initializing dialogue options.
        while (!cameraMan.InDesiredPosition())
        {
            yield return null;
        }

        DialogueBubbleUI.instance.init(dialogue);

        //Dictionary<string, DialogueAnimator> speakerDict = DialogueEngine.GetSpeakers(dialogue.text).ToDictionary(x => x, x => GameObject.Find(x).GetComponent<DialogueAnimator>());

        int lineTracker = 0;
        GameObject speaker = null;
        //string currentSpeaker = "";
        while (!dialogue.IsFinished)
        {
            ScriptLine line = dialogue.GetNextLine();
            GameObject newSpeaker = GameObject.Find(line.speaker);
            if (newSpeaker != null)
            {
                if(newSpeaker != speaker)
                {
                    speaker?.GetComponentInChildren<CharacterDialogueAnimator>()?.stopTalking();
                    speaker = newSpeaker;
                }
                speaker.GetComponentInChildren<CharacterDialogueAnimator>()?.startTalking();
                speaker.GetComponentInChildren<CharacterDialogueAnimator>()?.Turn(focusPosition.x - speaker.transform.position.x);
            }

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
        CharacterDialogueAnimator playerCda = player.GetComponent<CharacterDialogueAnimator>();
        playerCda.stopTalking();
        playerCda.changeExpression(CharacterExpression.normal);

        while (!DialogueBubbleUI.instance.ready)
            yield return null;

        foreach(string speakerName in dialogue.speakers)
        {
            GameObject newSpeaker = GameObject.Find(speakerName);
            speaker?.GetComponentInChildren<CharacterDialogueAnimator>()?.stopTalking();
        }


        cameraMan.EndCinematicMode();
        // No need to wait for CameraMan

        ConversationUnpause();

        afterEvent();
        yield return null;
    }

    //hacky gameplay pause implementation because hackathon
    private void ConversationPause()
    {
        PauseGameplay();
    }
    private void ConversationUnpause()
    {
        ResumeGameplay();
    }


    private void SetNpcCollisionBoxes(GameObject npcObject, bool enabled)
    {
        if (npcObject == null)
        {
            return;
        }

        var allColiders = npcObject.GetComponentsInChildren<Collider>();
        if (allColiders == null)
        {
            return;
        }

        // Assumption that NPCs don't have rigidbodies, and won't fall through the floor if we disable their colliders
        var colliders = allColiders.Where(c => !c.isTrigger);
        foreach (Collider c in colliders)
        {
            c.enabled = enabled;
        }
    }

    // Dependent on external player class
    int pauseCount = 0;
    public bool Paused { get { return pauseCount > 0; } }
    public void PauseGameplay()
    {
        pauseCount++;
        if (Paused)
        {
            foreach (CharacterMovement entity in GameObject.FindObjectsOfType<CharacterMovement>())
                entity.enabled = false;

            foreach (DialoguePromptTrigger dialogueTrigger in GameObject.FindObjectsOfType<DialoguePromptTrigger>())
                dialogueTrigger.triggerActive = false;
        }
    }
    public void ResumeGameplay()
    {
        pauseCount--;
        pauseCount = Mathf.Max(pauseCount, 0);
        if(!Paused)
        {
            foreach (CharacterMovement entity in GameObject.FindObjectsOfType<CharacterMovement>())
                entity.enabled = true;

            foreach (DialoguePromptTrigger dialogueTrigger in GameObject.FindObjectsOfType<DialoguePromptTrigger>())
                dialogueTrigger.triggerActive = true;
        }
    }

}
