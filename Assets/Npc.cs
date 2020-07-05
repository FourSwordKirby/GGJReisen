using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public List<NpcDialogue> dialogues;
    public DialoguePromptTrigger dialoguePromptTrigger;

    public void InitNpcState(CharacterProgress characterProgress)
    {
        dialoguePromptTrigger.dialogue = dialogues.Find(x => x.stage == characterProgress.GetStage()).dialogue;
    }
}

[Serializable]
public class NpcDialogue
{
    public int stage;
    public TextAsset dialogue;
}