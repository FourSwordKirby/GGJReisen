using JetBrains.Annotations;
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
}

[Serializable]
public class NpcDialogue
{
    public int stage;
    public TextAsset dialogue;
}