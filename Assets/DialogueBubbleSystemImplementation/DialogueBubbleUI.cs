using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueBubbleUI : MonoBehaviour
{
    private List<DialogueBubble> dialogueBubbles = new List<DialogueBubble>();
    private int onScreenSpeechBubbleLimit = 2;
    private int currentLineNumber = 0;
    private int z_offset = 1;

    public Dialogue activeDialogue;
    public bool ready;

    private List<float> randomSeedsX;
    private List<float> randomSeedsY;

    public static DialogueBubbleUI instance;

    public Camera dialogueCamera;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);
    }

    //This hacky implementation is kind of bad but I really don't want to mess with making the animator do this
    public void init(Dialogue dialogue)
    {
        currentLineNumber = dialogue.currentPosition;
        activeDialogue = dialogue;
        int speakingLineCount = activeDialogue.speakingLineCount;

        dialogueBubbles = new List<DialogueBubble>(speakingLineCount);
        randomSeedsX = new List<float>(speakingLineCount);
        randomSeedsY = new List<float>(speakingLineCount);

        for (int i = 0; i < speakingLineCount; i++)
        {
            //DialogueBubble speechBubble = DialogueUIController.GenerateSpeechBubblePrefab();
            //speechBubbles.Add(speechBubble);
            randomSeedsX.Add(Random.Range(-1.0f, 1.0f));
            randomSeedsY.Add(Random.Range(0f, 1.0f));
        }
    }

    public void finishDialogue()
    {
        ready = false;
        for (int i = 0; i < dialogueBubbles.Count; i++)
            dialogueBubbles[i].Hide();
        StartCoroutine(cleanup());
    }

    private IEnumerator cleanup()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < dialogueBubbles.Count; i++)
            dialogueBubbles[i].Destroy();
        ready = true;
    }

    // Kinda hacked together choice system
    internal void DisplayChoices(ChoiceLine line, Vector3 speakerPosition, DialogueBubbleType type = DialogueBubbleType.Thought)
    {
        ready = false;

        List<ChoiceLineContent> choices = line.dialogueChoices;

        int choicesCount = choices.Count;
        int lineNumber = choices[0].lineNumber;
        Vector2 speakerScreenPosition = dialogueCamera.WorldToScreenPoint(speakerPosition);

        float relativeXdisplacment = (Camera.main.pixelWidth / 2.0f - speakerScreenPosition.x) / Camera.main.pixelWidth;
        float relativeYdisplacment = (Camera.main.pixelHeight / 2.0f - speakerScreenPosition.y) / Camera.main.pixelHeight + 0.1f; // The dialogue should always be in the upper portion of the screen 
        Vector2 displacementVector = new Vector2(relativeXdisplacment, relativeYdisplacment);

        ChoiceBubble choiceBubble = DialogueUIController.GenerateChoiceBubblePrefab(choices.Count(), type);
        for(int i = 0; i < choices.Count; i++)
        {
            ChoiceLineContent content = choices[i];
            choiceBubble.choicePanels[i].SetChoicePanelContent(content);
        }

        dialogueBubbles.Add(choiceBubble);

        DialogueUIController.DeployDialogueBubbleAt(choiceBubble, speakerPosition, displacementVector);

        StartCoroutine(toggleChoices(line, choiceBubble));
        StartCoroutine(animateLogs(lineNumber));
    }

    //Displays the a speech bubble according to its text and position in the overall dialogue
    public void DisplaySpeechBubble(SpeakingLineContent speakingLineContent, Vector3 speakerPosition, DialogueBubbleType type = DialogueBubbleType.Speech)
    {
        ready = false;

        int lineNumber = speakingLineContent.lineNumber;
        Vector2 speakerScreenPosition = dialogueCamera.WorldToScreenPoint(speakerPosition);

        float relativeXdisplacment = (Camera.main.pixelWidth / 2.0f - speakerScreenPosition.x) / Camera.main.pixelWidth;
        float relativeYdisplacment = (Camera.main.pixelHeight / 2.0f - speakerScreenPosition.y) / Camera.main.pixelHeight + 0.1f; // The dialogue should always be in the upper portion of the screen 
        Vector2 displacementVector = new Vector2(relativeXdisplacment, relativeYdisplacment);

        // *(offscreen dialogue we will need to handle seperately)
        SpeechBubble speechBubble = DialogueUIController.GenerateSpeechBubblePrefab(type);
        speechBubble.SetDialogueBubbleContent(speakingLineContent);
        dialogueBubbles.Add(speechBubble);

        DialogueUIController.DeployDialogueBubbleAt(speechBubble, speakerPosition, displacementVector);

        StartCoroutine(animateLogs(lineNumber));
    }

    // kinda hacky crap aaa
    public IEnumerator toggleChoices(ChoiceLine choiceLine, ChoiceBubble choiceBubble)
    {
        while(!ready)
        {
            InputDirection dir = Controls.getInputDirectionDown();
            if (dir == InputDirection.N)
            {
                choiceLine.PreviousOption();
                choiceBubble.UpdateOption(choiceLine.GetOptionIndex());
            }
            else if (dir == InputDirection.S)
            {
                choiceLine.NextOption();
                choiceBubble.UpdateOption(choiceLine.GetOptionIndex());
            }
            yield return null;
        }
    }

    //Automatically animates the logs to the current state of the underlying dialogue data structure
    public IEnumerator animateLogs(int targetLineNumber)
    {
        // a bunch of the log advance/roll backwards stuff just isn't going to work with the current branching choice system
        // hacking it out so we always are looking at the latest dialogue
        currentLineNumber = dialogueBubbles.Count - 2;
        targetLineNumber = dialogueBubbles.Count - 1;

        if (currentLineNumber != targetLineNumber)
        {
            int offset = targetLineNumber - currentLineNumber;
            
            //crappy concurrency lol
            float logTweenTime = 0.2f;
            float delta = z_offset / logTweenTime * Time.deltaTime;

            while (logTweenTime > 0)
            {
                logTweenTime -= Time.deltaTime;
                for (int i = 0; i < dialogueBubbles.Count; i++)
                {
                    DialogueBubble animatedSpeechBubble = dialogueBubbles[i];
                    if(offset > 0)
                    {
                        if (targetLineNumber - offset <= i && i < targetLineNumber)
                            animatedSpeechBubble.transform.position += (Vector3.right * randomSeedsX[i] + Vector3.up * randomSeedsY[i]) * delta;
                    }
                    else
                    {
                        if (targetLineNumber <= i && i < targetLineNumber - offset)
                            animatedSpeechBubble.transform.position -= (Vector3.right * randomSeedsX[i] + Vector3.up * randomSeedsY[i]) * delta;
                    }
                    if (targetLineNumber - onScreenSpeechBubbleLimit < i && i <= targetLineNumber)
                        DialogueUIController.DeployDialogueBubble(animatedSpeechBubble);
                    // a bunch of the log advance/roll backwards stuff just isn't going to work with the current branching choice system, commenting out for now
                    if (i <= targetLineNumber - onScreenSpeechBubbleLimit)
                        DialogueUIController.HideDialogueBubble(animatedSpeechBubble);
                    if (i > targetLineNumber)
                        DialogueUIController.HideDialogueBubble(animatedSpeechBubble);
                    if (i == targetLineNumber)
                        animatedSpeechBubble.Focus();
                    int currentPosition = Mathf.Clamp(currentLineNumber - i, 0, onScreenSpeechBubbleLimit);
                    int targetPosition = Mathf.Clamp(targetLineNumber - i, 0, onScreenSpeechBubbleLimit);
                    animatedSpeechBubble.transform.position += (targetPosition - currentPosition) * Vector3.forward * delta;
                }
                yield return null;
            }
            for (int i = 0; i < dialogueBubbles.Count; i++)
            {
                if (i != targetLineNumber)
                    dialogueBubbles[i].Blur();
            }
            currentLineNumber = targetLineNumber;
        }
        yield return new WaitForSeconds(0.2f);
        while (!Controls.confirmInputDown())
        {
            yield return null;
        }

        ready = true;
    }
}
