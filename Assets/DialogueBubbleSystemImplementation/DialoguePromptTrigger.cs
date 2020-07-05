using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialoguePromptTrigger : MonoBehaviour
{
    public TextAsset dialogue;
    public bool repeatingDialogue;
    public bool forceDialogueOnEnter = false;
    public bool forceBack;
    public Transform promptPosition;
    public Transform cameraPosition;

    public UnityEvent postDialogueEvent;

    public List<DialogueInstruction> dialogueInstructions; 

    private Vector3 triggerEnteredPosition;
    private bool dialogueActive;
    private IDialogueBubble speechPrompt;

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
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
        hidePrompt();
    }

    void OnTriggerStay(Collider col)
    {
        if(Controls.confirmInputDown() && !dialogueActive)
        {
            displayDialogue();
        }
    }

    private void displayDialogue()
    {
        dialogueActive = true;
        hidePrompt();

        Vector3 speakerPosition = transform.position;

        if (repeatingDialogue)
            RpgGameManager.instance.StartConversation(dialogue, speakerPosition, cameraPosition, hideDialogue, dialogueInstructions);
        else
            RpgGameManager.instance.StartConversation(dialogue, speakerPosition, cameraPosition, destroy, dialogueInstructions);
    }
    
    private void hideDialogue()
    {
        postDialogueEvent?.Invoke();
        dialogueActive = false;
        if(!forceDialogueOnEnter)
            displayPrompt(Vector3.zero);
    }

    private void displayPrompt(Vector3 triggerEnteredPosition)
    {
        Vector3 displacementVector = Vector3.Scale(triggerEnteredPosition, -Vector3.one);
        speechPrompt = DialogueUIController.DisplaySpeechPrompt(promptPosition.position, displacementVector);
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
        Destroy(gameObject.transform.parent.gameObject);
    }
}
