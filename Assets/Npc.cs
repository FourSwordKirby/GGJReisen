using System;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public List<NpcDialogue> dialogues;
    public DialoguePromptTrigger dialoguePromptTrigger;
    public virtual ReisenNpcCharacterProgress NpcProgress
    {
        get
        {
            Debug.LogWarning($"A Generic NPC is not expected to have any CharacterProgress state. Be sure to override the {nameof(NpcProgress)} property for each specific character");
            return null;
        }
    }

    public int Stage
    {
        get
        {
            return NpcProgress.Stage;
        }
        set
        {
            NpcProgress.Stage = value;
            StageWasUpdated = true;
        }
    }

    public bool StageWasUpdated = false;

    public void InitNpcState(CharacterProgress characterProgress)
    {
        TextAsset dialogueTextForStage = dialogues.Find(x => x.stage == characterProgress.GetStage()).dialogue;
        dialoguePromptTrigger.SetDialogueText(dialogueTextForStage, !characterProgress.StageDialogueHasBeenRead);
    }

    public ReisenGameProgress GameProgress => ReisenGameManager.instance.gameProgress;

    public void AssignAvailableElixir(Assignment a)
    {
        if (GameProgress.Player.Elixir1 == Assignment.Inventory)
        {
            GameProgress.Player.Elixir1 = a;
        }
        else if (GameProgress.Player.Elixir2 == Assignment.Inventory)
        {
            GameProgress.Player.Elixir2 = a;
        }
        else
        {
            throw new InvalidOperationException("Tried to assign Elixir, but player does not have any in inventory");
        }
    }

    public void TransformToNormalSprite()
    {
        // TODO
        // For now a placeholder to know where we need it
    }

    public void DefaultDialogueEndEvent()
    {
        NpcProgress.DialogueRead = !StageWasUpdated;
        StageWasUpdated = false;
    }
}

[Serializable]
public class NpcDialogue
{
    public int stage;
    public TextAsset dialogue;
}