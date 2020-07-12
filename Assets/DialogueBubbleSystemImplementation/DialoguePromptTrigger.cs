using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DialogueTriggerSpeakerConfig
{
    None,
    Automatic,
    ForceLeft,
    ForceRight
}

public class DialoguePromptTrigger : MonoBehaviour
{
    public TextAsset dialogue;
    public bool triggerActive = true;
    public bool repeatingDialogue;
    public bool forceDialogueOnEnter = false;
    public bool forceBack;
    public Transform promptPosition;

    // Determines whether "!" or "..." should be displayed
    public bool useNewPrompt = false;
    // Set to true if the dropdown prompt icon should change between "!" and "..." after an interaction
    public bool trackNewDialogue = true;

    public DialogueTriggerSpeakerConfig speakingPositionConfig = DialogueTriggerSpeakerConfig.Automatic;
    public Transform leftSpeakingPosition;
    public Transform rightSpeakingPosition;
    public Transform cameraPosition;

    public UnityEvent postDialogueEvent;

    public List<DialogueInstruction> dialogueInstructions; 

    private Vector3 triggerEnteredPosition;
    private bool dialogueActive;
    private IDialogueBubble speechPrompt;

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (!triggerActive)
            return;

        triggerEnteredPosition = col.gameObject.transform.position;

        if (forceBack)
            col.attachedRigidbody.velocity =(col.transform.position - transform.position) * 2;

        if (forceDialogueOnEnter)
        {
            displayDialogue();
        }
        else
        {
            displayPrompt(promptPosition.position - triggerEnteredPosition);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (!triggerActive)
            return;

        hidePrompt();
    }

    void OnTriggerStay(Collider col)
    {
        if (!triggerActive)
            return;

        if (Controls.confirmInputDown() && !dialogueActive)
        {
            displayDialogue();
        }
    }

    private void displayDialogue()
    {
        dialogueActive = true;
        hidePrompt();

        Vector3 speakerPosition = transform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 displacement = (player.transform.position - this.transform.position);

        Transform speakingPosition = null;
        switch (speakingPositionConfig)
        {
            case DialogueTriggerSpeakerConfig.None:
                break;
            case DialogueTriggerSpeakerConfig.Automatic:
                speakingPosition = displacement.x < 0 ? leftSpeakingPosition : rightSpeakingPosition;
                break;
            case DialogueTriggerSpeakerConfig.ForceLeft:
                speakingPosition = leftSpeakingPosition;
                break;
            case DialogueTriggerSpeakerConfig.ForceRight:
                speakingPosition = rightSpeakingPosition;
                break;
        }

        Vector3 focusPosition;
        if (speakingPosition != null)
            focusPosition = (speakerPosition + speakingPosition.position) * 0.5f;
        else
            focusPosition = speakerPosition;

        if (repeatingDialogue)
            RpgGameManager.instance.StartConversation(dialogue, focusPosition, speakingPosition, cameraPosition, hideDialogue, dialogueInstructions);
        else
            RpgGameManager.instance.StartConversation(dialogue, focusPosition, speakingPosition, cameraPosition, destroy, dialogueInstructions);
    }

    private void hideDialogue()
    {
        postDialogueEvent?.Invoke();

        // Bad game specific hack that should get addressed somehwere else
        ReisenGameManager.instance.InitSceneState();

        dialogueActive = false;
        if(!forceDialogueOnEnter)
            displayPrompt(Vector3.zero);
    }

    private void displayPrompt(Vector3 triggerEnteredPosition)
    {
        Vector3 displacementVector = Vector3.Scale(triggerEnteredPosition, -Vector3.one);
        speechPrompt = DialogueUIController.DisplaySpeechPrompt(promptPosition.position, displacementVector, useNewPrompt);
    }


    private void hidePrompt()
    {
        if (speechPrompt == null)
        {
            return;
        }

        DialogueUIController.HideSpeechPrompt(speechPrompt);
    }

    private void destroy()
    {
        postDialogueEvent?.Invoke();

        // Bad game specific hack that should get addressed somehwere else
        ReisenGameManager.instance.InitSceneState();

        Destroy(gameObject.transform.parent.gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green - Color.black * 0.7f;
        Gizmos.DrawCube(this.transform.position, this.transform.lossyScale);
    }

    public void SetDialogueText(TextAsset newText, bool isNew)
    {
        SetDialogueAsNew(isNew);
        dialogue = newText;
    }

    public void SetDialogueAsNew(bool isNew)
    {
        if (trackNewDialogue)
        {
            useNewPrompt = isNew;
        }
    }
}
