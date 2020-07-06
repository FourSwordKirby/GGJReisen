using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    public GameObject speechBubblePrefab;
    public GameObject speechPromptPrefab;
    public GameObject speechChoicePrefab;

    public static GameObject staticSpeechBubblePrefab;
    public static GameObject staticSpeechPromptPrefab;
    public static GameObject staticSpeechChoicePrefab;

    public static DialogueUIController instance;
    internal static float deltaTime = 1.0f/60.0f;

    public void Awake()
    {
        if (DialogueUIController.instance == null)
            instance = this;
        else if (this != instance)
            Destroy(this.gameObject);

        staticSpeechBubblePrefab = speechBubblePrefab;
        staticSpeechPromptPrefab = speechPromptPrefab;
        staticSpeechChoicePrefab = speechChoicePrefab;
    }

    public static IDialogueBubble DisplaySpeechPrompt(Vector3 speakerPosition, Vector2 displacementVector)
    {
        IDialogueBubble speechPrompt = Instantiate(staticSpeechPromptPrefab).GetComponent<IDialogueBubble>();
        speechPrompt.DeployAt(speakerPosition, displacementVector);

        return speechPrompt;
    }

    public static void HideSpeechPrompt(IDialogueBubble speechPrompt)
    {
        speechPrompt.Hide();
    }

    public static void DeployDialogueBubbleAt(DialogueBubble dialogueBubble, Vector3 speakerPosition, Vector2 displacementVector)
    {
        dialogueBubble.gameObject.SetActive(true);
        dialogueBubble.DeployAt(speakerPosition, displacementVector);
    }

    public static void DeployDialogueBubble(IDialogueBubble dialogueBubble)
    {
        dialogueBubble.Show();
    }

    public static void HideDialogueBubble(IDialogueBubble dialogueBubble)
    {
        Debug.Log("hide");
        dialogueBubble.Blur();
        dialogueBubble.Hide();
    }

    public static SpeechBubble GenerateSpeechBubblePrefab(DialogueBubbleType type = DialogueBubbleType.Speech)
    {
        SpeechBubble speechBubble = Instantiate(staticSpeechBubblePrefab).GetComponent<SpeechBubble>();
        speechBubble.SetDialogueBubbleType(type);
        speechBubble.gameObject.SetActive(false);
        return speechBubble;
    }

    internal static ChoiceBubble GenerateChoiceBubblePrefab(int choiceCount, DialogueBubbleType type = DialogueBubbleType.Thought)
    {
        ChoiceBubble choiceBubble = Instantiate(staticSpeechChoicePrefab).GetComponent<ChoiceBubble>();
        choiceBubble.SetDialogueBubbleType(type);
        choiceBubble.Instantiate(choiceCount);
        choiceBubble.gameObject.SetActive(false);
        return choiceBubble;
    }
}
